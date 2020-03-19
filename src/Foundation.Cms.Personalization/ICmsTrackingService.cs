using EPiServer.Core;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Cms.Personalization
{
    public interface ICmsTrackingService
    {
        Task HeroBlockClickedAsync(HttpContextBase context, string blockId, string blockName, string pageName);
        Task VideoBlockViewedAsync(HttpContextBase context, string blockId, string blockName, string pageName);
        Task SearchedKeywordAsync(HttpContextBase httpContextBase, string keyword);
        Task BlockViewedAsync(BlockData block, IContent page, HttpContextBase httpContext);
        Task ImageViewedAsync(ImageData image, IContent page, HttpContextBase httpContext);
    }
}