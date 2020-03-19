using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Order;
using EPiServer.Personalization.Commerce.Tracking;
using EPiServer.Tracking.Commerce.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Commerce.Personalization
{
    public interface ICommerceTrackingService
    {
        Task<TrackingResponseData> TrackProductAsync(HttpContextBase httpContext, string productCode,
            bool skipRecommendations);

        Task<TrackingResponseData> TrackSearchAsync(HttpContextBase httpContext, string searchTerm, int pageSize,
            IEnumerable<string> productCodes);

        Task<TrackingResponseData> TrackOrderAsync(HttpContextBase httpContext, IPurchaseOrder order);
        Task<TrackingResponseData> TrackCategoryAsync(HttpContextBase httpContext, NodeContent category);
        Task<TrackingResponseData> TrackCartAsync(HttpContextBase httpContext, ICart cart);
        Task<TrackingResponseData> TrackWishlistAsync(HttpContextBase httpContext);
        Task<TrackingResponseData> TrackCheckoutAsync(HttpContextBase httpContext);
        Task<TrackingResponseData> TrackHomeAsync(HttpContextBase httpContext);
        Task<TrackingResponseData> TrackBrandAsync(HttpContextBase httpContext, string brandName);

        Task<TrackingResponseData> TrackAttributeAsync(HttpContextBase httpContext, string attributeName,
            string attributeValue);

        Task<TrackingResponseData> TrackDefaultAsync(HttpContextBase httpContext);
        IEnumerable<RecommendedProductTileViewModel> GetRecommendedProductTileViewModels(IEnumerable<Recommendation> recommendations);
    }
}