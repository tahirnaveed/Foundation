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

        public FoundationFacetGroupModifier(IVisitorGroupRepository visitorGroupRepository)
        {
            _visitorGroupRepository = visitorGroupRepository;
            var thisMonth = new DateTime(DateTime.Today.Year, DateTime.Now.Month, 1);
            _months.Add(thisMonth);
            for (int i = 1; i < 13; i++)
            {
                _months.Add(thisMonth.AddMonths(i));
            }
        }

        public override IEnumerable<FacetGroup> ModifyFacetGroups(IEnumerable<FacetGroup> facetGroups)
        {

            var facetGroupList = new List<FacetGroup>(facetGroups);

            facetGroupList.Add(new FacetGroup("visitorgroups", "Visitor Groups",
               _visitorGroupRepository.List().Select(x => new FacetItem(x.Id.ToString(), x.Name)).ToList(),
                 new FacetGroupSettings(FacetSelectionType.Multiple, 5, true, false, false, Enumerable.Empty<string>())));

            facetGroupList.Add(new FacetGroup("campaigndates", "Campaign Dates",
               _months.Select(x => new FacetItem(x.Ticks.ToString(), x.ToString("y"))).ToList(),
                 new FacetGroupSettings(FacetSelectionType.Multiple, 5, true, false, false, Enumerable.Empty<string>())));

            return facetGroupList;
        }
    }
}
