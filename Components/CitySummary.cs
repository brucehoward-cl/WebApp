using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc.ViewComponents;  // used for returning the HtmlContentViewComponendResult type
using Microsoft.AspNetCore.Html;    // used for returning the HtmlContentViewComponendResult type


namespace WebApp.Components
{
    public class CitySummary : ViewComponent
    {
        private CitiesData data;
        public CitySummary(CitiesData cdata)
        {
            data = cdata;
        }

        #region Prior to using a ViewModel
        //public string Invoke()
        //{
        //    return $"{data.Cities.Count()} cities, " + $"{data.Cities.Sum(c => c.Population)} people";
        //} 
        #endregion

        #region using the ViewViewComponentReseult return type (which implements IViewComponentResult)
        //public IViewComponentResult Invoke()
        //{
        //    return View(new CityViewModel
        //    {
        //        Cities = data.Cities.Count(),
        //        Population = data.Cities.Sum(c => c.Population)
        //    });
        //} 
        #endregion

        #region using the ContentViewComponentResult type (which implements IViewComponentResult)
        //public IViewComponentResult Invoke()
        //{
        //    return Content("This is a <h3><i>string</i></h3>"); // Content will make the input string safe (eg. by preventing javascript content from being embedded)
        //} 
        #endregion

        #region using the HtmlContentViewComponentResult type (which implements IViewComponentResult)
        //public IViewComponentResult Invoke()
        //{
        //    return new HtmlContentViewComponentResult(new HtmlString("This is a <h3><i>string</i></h3>"));
        //} 
        #endregion

        #region using the Context data to get information about the request/response
        //public string Invoke()
        //{
        //    if (RouteData.Values["controller"] != null)
        //    {
        //        return "Controller Request";
        //    }
        //    else
        //    {
        //        return "Razor Page Request";
        //    }
        //} 
        #endregion

        #region using Context data provided by the parent view (or to the view?)
        public IViewComponentResult Invoke(string themeName)
        {
            ViewBag.Theme = themeName;
            return View(new CityViewModel
            {
                Cities = data.Cities.Count(),
                Population = data.Cities.Sum(c => c.Population)
            });
        } 
        #endregion
    }

}
