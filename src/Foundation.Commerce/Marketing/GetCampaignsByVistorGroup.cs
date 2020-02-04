using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Marketing
{
    public class GetCampaignsByVistorGroup : GetContentsByFacet
    {
        public override string Key { get { return "visitorgroup"; } }

        public override IEnumerable<IContent> GetItems(IEnumerable<IContent> items, IEnumerable<string> facets)
        {
            return items.Where(item => !(item is SalesCampaign) || AvailableForVistorGroups((SalesCampaign)item, facets));
        }
        
        private bool AvailableForVistorGroups(SalesCampaign campaign, IEnumerable<string> facets)
        {
            return campaign.VisitorGroups == null
                || campaign.VisitorGroups.Count == 0
                || campaign.VisitorGroups.Any(x => facets.Contains(x.ToString()));
        }
    }
}
