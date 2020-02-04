using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Marketing
{
    public class GetCampaignsByDates : GetContentsByFacet
    {
        public override string Key { get { return "campaigndates"; } }

        public override IEnumerable<IContent> GetItems(IEnumerable<IContent> items, IEnumerable<string> facets)
        {
            return items.Where(item => !(item is SalesCampaign) || AvailableForDates((SalesCampaign)item, facets));
        }

        private bool AvailableForDates(SalesCampaign campaign, IEnumerable<string> facets)
        {
            foreach (var facet in facets)
            {
                var checkMonth = new DateTime(Convert.ToInt64(facet));
                if (checkMonth >= campaign.ValidFrom && checkMonth <= campaign.ValidUntil)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
