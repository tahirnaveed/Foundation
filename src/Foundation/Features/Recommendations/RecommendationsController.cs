using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Commerce.Personalization;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Recommendations
{
    public class RecommendationsController : Controller
    {
        private readonly ICommerceTrackingService _recommendationService;
        private readonly ReferenceConverter _referenceConverter;

        public RecommendationsController(ICommerceTrackingService recommendationService, ReferenceConverter referenceConverter)
        {
            _recommendationService = recommendationService;
            _referenceConverter = referenceConverter;
        }

        public async Task<JsonResult> RecommendationApi(string customer)
        {
            if (!string.IsNullOrWhiteSpace(customer))
            {
                HttpContext.User = new GenericPrincipal(new GenericIdentity(customer), new[] { "" });
            }

            var trackingResponse = await _recommendationService.TrackHome(HttpContext);
            var recommendations = trackingResponse.GetRecommendations(_referenceConverter, RecommendationsExtensions.Home);
            var productModels = _recommendationService.GetRecommendedProductTileViewModels(recommendations);

            return Json(productModels.Select(x => new
            {
               x.TileViewModel.ImageUrl,
               x.TileViewModel.DisplayName,
               x.TileViewModel.Url,
               PlacedPrice = x.TileViewModel.PlacedPrice.ToString(),
               x.TileViewModel.Description,
               x.TileViewModel.Brand,
               x.TileViewModel.Code,
               x.TileViewModel.IsAvailable,
               x.TileViewModel.IsFeaturedProduct
            }), JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult Index(IEnumerable<Recommendation> recommendations)
        {
            if (recommendations == null || !recommendations.Any())
            {
                return new EmptyResult();
            }

            if (recommendations.Count() > 4)
            {
                recommendations = recommendations.Take(4);
            }

            return PartialView("Index", _recommendationService.GetRecommendedProductTileViewModels(recommendations));
        }
    }
}