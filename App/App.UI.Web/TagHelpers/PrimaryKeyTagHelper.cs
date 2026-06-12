using Microsoft.AspNetCore.Razor.TagHelpers;

namespace App.UI.Web.TagHelpers
{
    [HtmlTargetElement("*", Attributes = "asp-key")]
    public class PrimaryKeyTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-key")]
        public bool IsPrimaryKey { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsPrimaryKey)
            {
                output.Attributes.SetAttribute("asp-key", "true");
            }
        }
    }
}
