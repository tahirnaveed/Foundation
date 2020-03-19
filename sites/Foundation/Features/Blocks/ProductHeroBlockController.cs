using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Commerce.Blocks;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class ProductHeroBlockController : BlockController<ProductHeroBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;

        public ProductHeroBlockController(IContentLoader contentLoader, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        [HttpGet]
        public override ActionResult Index(ProductHeroBlock currentContent)
        {
            var imageUrl = string.Empty;
            var imagePosition = string.Empty;
            

            if (currentContent.Image.Product != null)
            {
                var entryContentBase = _contentLoader.Get<EntryContentBase>(currentContent.Image.Product.Items[0].ContentLink);
                imageUrl = entryContentBase.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault() ?? string.Empty;
            }

            if (currentContent.Image.ImagePosition.Equals("ImageLeft", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition = "justify-content: flex-start;";
            }
            else if (currentContent.Image.ImagePosition.Equals("ImageCenter", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition = "justify-content: center;";
            }
            else if (currentContent.Image.ImagePosition.Equals("ImageRight", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition = "justify-content: flex-end;";
            }
            else if (currentContent.Image.ImagePosition.Equals("ImagePaddings", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition = "padding: "
                    + currentContent.Image.PaddingTop + "px "
                    + currentContent.Image.PaddingRight + "px "
                    + currentContent.Image.PaddingBottom + "px "
                    + currentContent.Image.PaddingLeft + "px;";
            }

            var model = new ProductHeroBlockViewModel(currentContent)
            {
                ImageUrl = imageUrl,
                ImagePosition = imagePosition
            };

            return PartialView("~/Features/Blocks/Views/ProductHeroBlock.cshtml", model);
        }
    }
}
