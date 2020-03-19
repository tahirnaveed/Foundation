using EPiServer;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Mvc;
using Foundation.Cms.Categories;
using Foundation.Find.Cms.Locations.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Locations.LocationItemPage
{
    public class LocationItemPageController : PageController<Find.Cms.Models.Pages.LocationItemPage>
    {
        private readonly IContentRepository _contentRepository;

        public LocationItemPageController(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        [HttpGet]
        public ActionResult Index(Find.Cms.Models.Pages.LocationItemPage currentPage)
        {
            var model = new LocationViewModel(currentPage);
            if (!ContentReference.IsNullOrEmpty(currentPage.Image))
            {
                model.Image = _contentRepository.Get<ImageData>(currentPage.Image);
            }

            model.LocationNavigation.ContinentLocations = SearchClient.Instance
                .Search<Find.Cms.Models.Pages.LocationItemPage>()
                .Filter(x => x.Continent.Match(currentPage.Continent))
                .PublishedInCurrentLanguage()
                .OrderBy(x => x.PageName)
                .FilterForVisitor()
                .Take(100)
                .StaticallyCacheFor(new System.TimeSpan(0, 10, 0))
                .GetContentResult();

            model.LocationNavigation.CloseBy = SearchClient.Instance
                .Search<Find.Cms.Models.Pages.LocationItemPage>()
                .Filter(x => x.Continent.Match(currentPage.Continent)
                             & !x.PageLink.Match(currentPage.PageLink))
                .PublishedInCurrentLanguage()
                .FilterForVisitor()
                .OrderBy(x => x.Coordinates)
                .DistanceFrom(currentPage.Coordinates)
                .Take(5)
                .StaticallyCacheFor(new System.TimeSpan(0, 10, 0))
                .GetContentResult();

            if (currentPage.Categories != null)
            {
                model.Tags = currentPage.Categories.Select(x => _contentRepository.Get<StandardCategory>(x));
            }

            var editingHints = ViewData.GetEditHints<LocationViewModel, Find.Cms.Models.Pages.LocationItemPage>();
            editingHints.AddFullRefreshFor(p => p.Image);
            editingHints.AddFullRefreshFor(p => p.Categories);

            return View(model);
        }

        public virtual string SearchTextFly(Find.Cms.Models.Pages.LocationItemPage currentPage)
        {
            var searchText = "";

            return searchText;
        }
    }
}