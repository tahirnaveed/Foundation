using EPiServer.Commerce.Shell.Facets;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.Shell.Services.Rest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Marketing
{
    [RestStore("facet")]
    public class FoundationFacetGroupModifier : FacetGroupModifier
    {
        private readonly IVisitorGroupRepository _visitorGroupRepository;
        private readonly List<DateTime> _months = new List<DateTime>();
        private readonly List<string> _dependencies = new List<string>
        {
            
        };

        public FoundationFacetGroupModifier(IVisitorGroupRepository visitorGroupRepository)
        {
            _visitorGroupRepository = visitorGroupRepository;
            var month = new DateTime(DateTime.Today.Year, DateTime.Now.Month, 1).AddMonths(-7);
            _months.Add(month);
            for (int i = 1; i < 15; i++)
            {
                _months.Add(month.AddMonths(i));
            }
        }

        public override IEnumerable<FacetGroup> ModifyFacetGroups(IEnumerable<FacetGroup> facetGroups)
        {

            var facetGroupList = new List<FacetGroup>(facetGroups);

            facetGroupList.Add(new FacetGroup(GetCampaignsByVistorGroup.VisitorGroups, "Visitor Groups",
               _visitorGroupRepository.List().Select(x => new FacetItem(x.Id.ToString(), x.Name)).ToList(),
               new FacetGroupSettings(FacetSelectionType.Multiple, 5, true, false, true, new[] { CampaignFacetConstants.StatusGroupId, CampaignFacetConstants.MarketGroupId, CampaignFacetConstants.DiscountTypeGroupId, GetPromotionsByDates.PromotionDates })));

            facetGroupList.Add(new FacetGroup(GetPromotionsByDates.PromotionDates, "Promotion Dates",
               _months.Select(x => new FacetItem(x.Ticks.ToString(), x.ToString("y"))).ToList(),
               new FacetGroupSettings(FacetSelectionType.Multiple, 0, false, false, true, new[] { CampaignFacetConstants.StatusGroupId, CampaignFacetConstants.MarketGroupId, CampaignFacetConstants.DiscountTypeGroupId, GetCampaignsByVistorGroup.VisitorGroups })));

            return facetGroupList;
        }
    }
}
