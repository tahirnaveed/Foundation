using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Cms.Blocks;
using Foundation.Cms.ViewModels;
using System;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true, Inherited = true)]
    public class DefaultBlockController : BlockController<FoundationBlockData>
    {
        [HttpGet]
        public override ActionResult Index(FoundationBlockData currentContent)
        {
            var model = CreateModel(currentContent);
            return PartialView(string.Format("~/Features/Blocks/Views/{0}.cshtml", currentContent.GetOriginalType().Name), model);
        }

        private static IBlockViewModel<BlockData> CreateModel(BlockData currentBlock)
        {
            var type = typeof(BlockViewModel<>).MakeGenericType(currentBlock.GetOriginalType());
            return Activator.CreateInstance(type, currentBlock) as IBlockViewModel<BlockData>;
        }
    }
}