using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Marketing
{
    public class GetCampaignsByVistorGroup : GetContentsByFacet
    {
        private readonly string _type;
        private readonly IContentLoader _contentLoader;
        public const string VisitorGroups = "visitorgroups";
        public override string Key { get { return VisitorGroups; } }

        public GetCampaignsByVistorGroup(IContentLoader contentLoader, string type)
        {
            _type = type;
            _contentLoader = contentLoader;
        }

        public override IEnumerable<IContent> GetItems(IEnumerable<IContent> items, IEnumerable<string> facets)
        {
            if (_type.Equals("children"))
            {
                return items.OfType<SalesCampaign>()
                    .SelectMany(x => _contentLoader.GetChildren<PromotionData>(x.ContentLink).Where(_ => AvailableForVistorGroups(x, facets)));
            }

            return items.Where(item => EvaluateTypes(item, facets));
        }

        private bool EvaluateTypes(IContent content, IEnumerable<string> filteredDiscountTypes)
        {
            if (content is PromotionData promotionData)
            {
                return AvailableForVistorGroups(_contentLoader.Get<SalesCampaign>(promotionData.ParentLink), filteredDiscountTypes);
            }
            else if (content is SalesCampaign salesCampaign)
            {
                return AvailableForVistorGroups(salesCampaign, filteredDiscountTypes);
            }

            return true;
        }
        
        private bool AvailableForVistorGroups(SalesCampaign campaign, IEnumerable<string> facets)
        {
            return campaign.VisitorGroups == null
                || campaign.VisitorGroups.Count == 0
                || campaign.VisitorGroups.Any(x => facets.Contains(x.ToString()));
        }
    }
}
