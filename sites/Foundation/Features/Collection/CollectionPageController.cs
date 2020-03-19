using EPiServer.Web.Mvc;
using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Web.Mvc;

namespace Foundation.Features.Collection
{
    public class CollectionPageController : PageController<CollectionPage>
    {
        [HttpGet]
        public ActionResult Index(CollectionPage currentPage) => View(ContentViewModel.Create(currentPage));
    }
}