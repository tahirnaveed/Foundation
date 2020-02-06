using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace Foundation.Features.MappableImage
{
    public class MappableImageBlockController : BlockController<MappableImageBlock>
    {
        public override ActionResult Index(MappableImageBlock currentBlock)
        {
            return PartialView(currentBlock);
        }
    }
}