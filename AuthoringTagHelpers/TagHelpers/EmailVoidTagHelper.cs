using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement("email",TagStructure = TagStructure.WithoutEndTag)]
    public class EmailVoidTagHelper:TagHelper
    {
        private const string EmailDomain = "contoso.com";
    }
}
