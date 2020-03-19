using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Blocks;
using Foundation.Cms.ViewModels.Blocks;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class NavigationBlockController : BlockController<NavigationBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IPageRouteHelper _pageRouteHelper;

        public NavigationBlockController(IContentLoader contentLoader, IPageRouteHelper pageRouteHelper)
        {
            _contentLoader = contentLoader;
            _pageRouteHelper = pageRouteHelper;
        }

        [HttpGet]
        public override ActionResult Index(NavigationBlock currentContent)
        {
            var rootNavigation = currentContent.RootPage as ContentReference;
            if (ContentReference.IsNullOrEmpty(currentContent.RootPage))
            {
                rootNavigation = _pageRouteHelper.ContentLink;
            }

            var childPages = _contentLoader.GetChildren<PageData>(rootNavigation);
            var model = new NavigationBlockViewModel(currentContent);
            if (childPages != null && childPages.Any())
            {
                var linkCollection = new List<NavigationItem>();
                foreach (var page in childPages)
                {
                    if (page.VisibleInMenu)
                    {
                        linkCollection.Add(new NavigationItem(page, Url));
                    }
                }

                model.Items.AddRange(linkCollection.Where(x => !string.IsNullOrEmpty(x.Url)));
            }

            if (string.IsNullOrEmpty(currentContent.Heading))
            {
                model.Heading = _pageRouteHelper.Page.Name;
            }

            return PartialView("~/Features/Blocks/Views/NavigationBlock.cshtml", model);
        }
    }
}