using EPiServer;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Marketing
{
    [ServiceConfiguration(typeof(IContentQuery))]
    public class GetSalesCampaignChildrenQuery : GetSalesCampaignQueryBase
    {
        private readonly IContentRepository _contentRepository2;
        private readonly IContentLoader _contentLoader;

        public GetSalesCampaignChildrenQuery(
            IContentQueryHelper queryHelper,
            IContentRepository contentRepository,
            LanguageSelectorFactory languageSelectorFactory,
            CampaignInfoExtractor campaignInfoExtractor,
            FacetQueryHandler facetQueryHandler,
            IContentLoader contentLoader)
            : base(queryHelper, contentRepository, languageSelectorFactory, campaignInfoExtractor, facetQueryHandler) {
            _contentRepository2 = contentRepository;
            _contentLoader = contentLoader;
        }

        /// <summary>
        /// Gets the rank of this query (100).
        /// </summary>
        public override int Rank
        {
            get { return 200; }
        }

        /// <inheritdoc />
        protected override IEnumerable<GetContentsByFacet> FacetFunctions
        {
            get
            {
                return new GetContentsByFacet[]
                {
                    new GetCampaignsByStatus(_campaignInfoExtractor),
                    new GetCampaignsByMarket(),
                    new GetCampaignsByDiscountType(_contentRepository2),
                    new GetPromotionsByDates(_contentLoader, "branch"),
                    new GetCampaignsByVistorGroup(_contentLoader, "branch")
                };
            }
        }

        /// <summary>
        /// Sorts the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected override IEnumerable<IContent> Sort(IEnumerable<IContent> items, ContentQueryParameters parameters)
        {
            // We'll be passing through the collection multiple times to be able to sort campaigns, promotions
            // and other items (if any) so execute the enumeration once.
            var itemsList = items.ToList();
            var sortedCampaignItems = new List<IContent>();

            var sortedCampaigns =
                itemsList.OfType<SalesCampaign>()
                    .OrderByDescending(c => c.IsActive)
                    .ThenBy(c => _campaignInfoExtractor.GetStatusFromDates(c.ValidFrom, c.ValidUntil));
            sortedCampaignItems.AddRange(sortedCampaigns);

            SalesCampaign parentCampaign;
            if (_contentRepository2.TryGet(parameters.ReferenceId, out parentCampaign) && parentCampaign != null)
            {
                var sortedPromotions = itemsList.OfType<PromotionData>()
                    .OrderBy(p => _campaignInfoExtractor.GetEffectiveStatus(p, parentCampaign))
                    .ThenBy(p => _campaignInfoExtractor.GetEffectiveValidFrom(p, parentCampaign))
                    .ThenByDescending(p => _campaignInfoExtractor.GetEffectiveValidUntil(p, parentCampaign));
                sortedCampaignItems.AddRange(sortedPromotions);
            }

            var otherItems = base.Sort(itemsList.Except(sortedCampaignItems), parameters);
            return sortedCampaignItems.Concat(otherItems);
        }
    }
}