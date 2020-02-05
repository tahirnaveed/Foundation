using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Marketing
{
    public class GetPromotionsByDates : GetContentsByFacet
    {
        private readonly IContentLoader _contentLoader;
        private readonly string _type;
        public const string PromotionDates = "promotiondates";

        public GetPromotionsByDates(IContentLoader contentLoader, string type)
        {
            _contentLoader = contentLoader;
            _type = type;
        }

        public override string Key { get { return PromotionDates; } }

        public override IEnumerable<IContent> GetItems(IEnumerable<IContent> items, IEnumerable<string> facets)
        {
            if (_type.Equals("children"))
            {
                return items.OfType<SalesCampaign>()
                    .SelectMany(x => _contentLoader.GetChildren<PromotionData>(x.ContentLink).Where(_ => AvailableForDates(facets, x)));
            }

            return items.Where(item => EvaluateTypes(item, facets));
        }

        private bool EvaluateTypes(IContent content, IEnumerable<string> filteredDiscountTypes)
        {
            if (content is PromotionData promotionData)
            {
                return AvailableForDates(filteredDiscountTypes, _contentLoader.Get<SalesCampaign>(promotionData.ParentLink));
            }
            else if (content is SalesCampaign salesCampaign)
            {
                return AvailableForDates(filteredDiscountTypes, salesCampaign);
            }

            return true;
        }

        private bool AvailableForDates(IEnumerable<string> facets, SalesCampaign salesCampaign)
        {
            return facets.Any(facet =>
            {
                var startOfMonth = new DateTime(Convert.ToInt64(facet));
                int numberOfDays = DateTime.DaysInMonth(startOfMonth.Year, startOfMonth.Month);
                var lastDay = new DateTime(startOfMonth.Year, startOfMonth.Month, numberOfDays, 23, 59, 59);

                return (startOfMonth >= salesCampaign.ValidFrom && startOfMonth <= salesCampaign.ValidUntil) ||
                       (lastDay >= salesCampaign.ValidFrom && lastDay <= salesCampaign.ValidUntil);
            });
        }
    }
}
