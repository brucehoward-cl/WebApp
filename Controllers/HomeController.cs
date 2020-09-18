using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private DataContext context;

        public HomeController(DataContext ctx)
        {
            context = ctx;
        }

        public async Task<IActionResult> Index(long id = 1)
        {
            ViewBag.AveragePrice = await context.Products.AverageAsync(p => p.Price);
            return View(await context.Products.FindAsync(id));
        }

        public IActionResult List()
        {
            return View(context.Products);
        }

        public IActionResult Html()
        {
            return View((object)"This is a <h3><i>string</i></h3>");
        }
    }

    #region Chapter 21
    //public class HomeController : Controller
    //{
    //    private DataContext context;

    //    public HomeController(DataContext ctx)
    //    {
    //        context = ctx;
    //    }
    //    public async Task<IActionResult> Index(long id = 1)
    //    {
    //        //return View(await context.Products.FindAsync(id));
    //        Product prod = await context.Products.FindAsync(id);
    //        if (prod.CategoryId == 1)
    //        {
    //            return View("Watersports", prod);
    //        }
    //        else
    //        {
    //            return View(prod);
    //        }
    //    }
    //    public IActionResult Common()
    //    {
    //        return View();
    //    }

    //    public IActionResult List()
    //    {
    //        return View(context.Products);
    //    }
    //} 
    #endregion
}