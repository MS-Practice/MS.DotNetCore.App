using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement("p")]
    public class AutoLinkerHttpTagHelper : TagHelper
    {
        // This filter must run before the AutoLinkerWwwTagHelper as it searches and replaces http and 
        // the AutoLinkerWwwTagHelper adds http to the markup.
        public override int Order => int.MinValue;
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(Regex.Replace(
                childContent.GetContent(),
                @"\b(?:https?://)(\S+)\b",  //\b只是匹配字符串开头结尾及空格回车等的位置 \S 表示非空白字符 \s表示空白字符    +表示一个或多个
                "<a target=\"_blank\" href=\"$0\">$0</a>"));
        }
    }
    [HtmlTargetElement("p")]
    public class AutoLinkerWWWTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            // Find Urls in the content and replace them with their anchor tag equivalent.
            output.Content.SetHtmlContent(Regex.Replace(
                childContent.GetContent(),
                 @"\b(www\.)(\S+)\b",
                 "<a target=\"_blank\" href=\"http://$0\">$0</a>"));  // www version
        }
    }
}
