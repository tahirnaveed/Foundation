using EPiServer.Web.Mvc;
using Foundation.Cms.ViewModels;
using Foundation.Demo.Models;
using System.Web.Mvc;

namespace Foundation.Features.Home
{
    public class HomeController : PageController<DemoHomePage>
    {
        [HttpGet]
        public ActionResult Index(DemoHomePage currentContent) => View(ContentViewModel.Create(currentContent));
    }
}