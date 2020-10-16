using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApp.TagHelpers
{
    [HtmlTargetElement("tr", Attributes = "theme")]
    public class RowTagHelper : TagHelper
    {
        public string Theme { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.Items["theme"] = Theme;  //This makes the value of the Theme attribute available to child elements
        }
    }

    [HtmlTargetElement("th")]
    [HtmlTargetElement("td")]
    public class CellTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context.Items.ContainsKey("theme"))
            {
                output.Attributes.SetAttribute("class",$"bg-{context.Items["theme"]} text-white");
            }
        }
    }
}