using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace AuthoringTagHelpers.TagHelpers
{
    //[HtmlTargetElement("bold", Attributes = "bold")]//逻辑与
    //逻辑或
    [HtmlTargetElement("bold")]
    [HtmlTargetElement(Attributes = "bold")]
    public class BoldTagHelper:TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("bold");
            output.PreContent.SetHtmlContent("<strong>");
            output.PostContent.SetHtmlContent("</strong>");
            await Task.CompletedTask;
        }
    }
}
