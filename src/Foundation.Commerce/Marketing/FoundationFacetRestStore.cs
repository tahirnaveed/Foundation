using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Facets;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;
using EPiServer.Shell.Services.Rest;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Commerce.Marketing
{
    public class FoundationFacetRestStore : RestControllerBase
    {
        private readonly CampaignInfoExtractor _campaignInfoExtractor;
        private readonly IContentLoader _contentLoader;
        private readonly FacetQueryHandler _facetQueryHandler;
        private readonly FacetGroupProvider _facetGroupProvider;
        private readonly FacetGroupModifier _facetGroupModifier;
        private IList<FacetGroup> _facetGroups;

        private readonly IEnumerable<GetContentsByFacet> _filters;
        private readonly object _lockObject = new object();

        public FoundationFacetRestStore(
            CampaignInfoExtractor campaignInfoExtractor,
            IContentLoader contentLoader,
            FacetQueryHandler facetQueryHandler,
            FacetGroupProvider facetGroupProvider,
            FacetGroupModifier facetGroupModifier)
        {
            _campaignInfoExtractor = campaignInfoExtractor;
            _contentLoader = contentLoader;
            _facetQueryHandler = facetQueryHandler;
            _facetGroupProvider = facetGroupProvider;
            _facetGroupModifier = facetGroupModifier;

            _filters = new GetContentsByFacet[]{
                new GetCampaignsByStatus(_campaignInfoExtractor),
                new GetCampaignsByMarket(),
                new GetPromotionsByDiscountType(_contentLoader),
                new GetCampaignsByDates(),
                new GetCampaignsByVistorGroup()
            };
        }

        /// <summary>
        /// The facet groups.
        /// </summary>
        public IList<FacetGroup> FacetGroups
        {
            get
            {
                if (_facetGroups == null)
                {
                    lock (_lockObject)
                    {
                        if (_facetGroups == null)
                        {
                            var builtinFacetGroups = _facetGroupProvider.GetFacetGroups();
                            _facetGroups = _facetGroupModifier.ModifyFacetGroups(builtinFacetGroups).ToList();
                        }
                    }
                }

                return _facetGroups;
            }
        }

        [HttpGet]
        public RestResultBase Get(string id, string facetString, ContentReference parentLink)
        {
            _facetQueryHandler.CalculateMatchingNumbers(_contentLoader.GetChildren<IContent>(parentLink), FacetGroups, facetString, _filters);
            var facet = new CampaignFacet(FacetGroups);
            return Rest(facet.Id == id ? facet : null);
        }
    }
}