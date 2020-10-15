using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebApp.Models;

namespace WebApp.Pages
{
    [ViewComponent(Name = "CitiesPageHybrid")]
    public class CitiesModel : PageModel
    {
        public CitiesModel(CitiesData cdata)
        {
            Data = cdata;
        }

        public CitiesData Data { get; set; }

        [ViewComponentContext]
        public ViewComponentContext Context { get; set; }
        
        public IViewComponentResult Invoke()
        {
            return new ViewViewComponentResult()
            {
                ViewData = new ViewDataDictionary<CityViewModel>(
                    Context.ViewData,
                    new CityViewModel
                    {
                        Cities = Data.Cities.Count(),
                        Population = Data.Cities.Sum(c => c.Population)
                    })
            };
        }
    }
}


#region code used prior to adding the view Component
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace WebApp.Pages
//{
//    public class CitiesModel : PageModel
//    {
//        public void OnGet()
//        {
//        }
//    }
//} 
#endregion


