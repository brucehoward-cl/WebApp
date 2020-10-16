using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApp.TagHelpers
{
    //[HtmlTargetElement("tr", Attributes = "bg-color,text-color", ParentTag = "thead")]  //narrows the scope of elements transformed by taghelper
    //[HtmlTargetElement("*", Attributes = "bg-color,text-color")]  //widens the scope of elements transformed by taghelper
    #region Using multiple HtmlTargetElement attributes together can target multiple elements more precisely
    //If you need to apply multiple tag helpers to an element, you can control the sequence in which they execute by setting the Order
    //property, which is inherited from the TagHelper base class
    [HtmlTargetElement("tr", Attributes = "bg-color,text-color")]
    [HtmlTargetElement("td", Attributes = "bg-color")] 
    #endregion
    public class TrTagHelper : TagHelper
    {
        public string BgColor { get; set; } = "dark";
        public string TextColor { get; set; } = "white";
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("class",$"bg-{BgColor} text-center text-{TextColor}");
            
        }
    }
}