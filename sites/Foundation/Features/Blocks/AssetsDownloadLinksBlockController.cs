using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Models.Blocks;
using Foundation.Commerce.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class AssetsDownloadLinksBlockController : BlockController<AssetsDownloadLinksBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;

        public AssetsDownloadLinksBlockController(IContentLoader contentLoader, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        [HttpGet]
        public override ActionResult Index(AssetsDownloadLinksBlock currentContent)
        {
            var model = new AssetsDownloadLinksBlockViewModel(currentContent);
            var rootContent = _contentLoader.Get<IContent>(currentContent.RootContent);
            if (rootContent != null)
            {
                var assets = new List<MediaData>();
                if (rootContent is ContentFolder)
                {
                    assets = _contentLoader.GetChildren<MediaData>(rootContent.ContentLink).OrderByDescending(x => x.StartPublish).ToList();
                }

                if (rootContent is IAssetContainer assetContainer)
                {
                    assets = assetContainer.GetAssetsMediaData(_contentLoader, currentContent.GroupName)
                        .OrderByDescending(x => x.StartPublish).ToList();
                }

                if (currentContent.Count > 0)
                {
                    assets = assets.Take(currentContent.Count).ToList();
                }

                model.Assets = assets;
            }

            return PartialView("~/Features/Blocks/Views/AssetsDownloadLinksBlock.cshtml", model);
        }

        public void Download(int contentLinkId)
        {
            if (_contentLoader.Get<IContent>(new ContentReference(contentLinkId)) is MediaData mediaData)
            {
                if (mediaData is MediaData downloadFile)
                {
                    if (downloadFile.BinaryData is FileBlob blob)
                    {
                        var routeSegment = downloadFile.RouteSegment;
                        var extension = Path.GetExtension(blob.FilePath) ?? "";
                        var downloadFileName = routeSegment.EndsWith(extension) ? routeSegment : routeSegment + extension;

                        HttpContext.Response.ContentType = "application/octet-stream";
                        HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(downloadFileName));
                        HttpContext.Response.TransmitFile(blob.FilePath);
                    }
                }
            }
        }
    }
}
