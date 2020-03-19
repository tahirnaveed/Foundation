using EPiBootstrapArea;
using EPiServer.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Cms.Display
{
    public class FoundationContentAreaRenderer : BootstrapAwareContentAreaRenderer
    {
        public FoundationContentAreaRenderer() : base(new FoundationDisplayModeProvider().GetAll())
        {
        }

        protected override void RenderContentAreaItems(HtmlHelper htmlHelper, IEnumerable<ContentAreaItem> contentAreaItems)
        {
            if (contentAreaItems == null)
            {
                throw new System.ArgumentNullException(nameof(contentAreaItems));
            }

            TagBuilder currentRow;

            foreach (var contentAreaItem in contentAreaItems)
            {
                string templateTag = GetContentAreaItemTemplateTag(htmlHelper, contentAreaItem);
                bool isScreenContentAreaItem = IsScreenWidthTag(templateTag);

                if (isScreenContentAreaItem)
                {
                    currentRow = new TagBuilder("div");
                    currentRow.AddCssClass("screen-width-block");
                    htmlHelper.ViewContext.Writer.Write(currentRow.ToString(TagRenderMode.StartTag));
                    RenderContentAreaItem(htmlHelper, contentAreaItem, templateTag, GetContentAreaItemHtmlTag(htmlHelper, contentAreaItem), GetContentAreaItemCssClass(htmlHelper, contentAreaItem, templateTag));
                    htmlHelper.ViewContext.Writer.Write(currentRow.ToString(TagRenderMode.EndTag));
                }
                else
                {
                    RenderContentAreaItem(htmlHelper, contentAreaItem, templateTag, GetContentAreaItemHtmlTag(htmlHelper, contentAreaItem), GetContentAreaItemCssClass(htmlHelper, contentAreaItem, templateTag));
                }
            }
        }

        protected virtual string GetContentAreaItemCssClass(HtmlHelper html, ContentAreaItem contentAreaItem, string templateTag)
        {
            var baseClass = base.GetContentAreaItemCssClass(html, contentAreaItem);

            if (!string.IsNullOrEmpty(baseClass))
            {
                return baseClass;
            }

            return string.Format("block {0}", templateTag);
        }

        protected virtual bool IsScreenWidthTag(string templateTag) => templateTag == "displaymode-screen";
    }
}
