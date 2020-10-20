using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Controllers
{
    [AutoValidateAntiforgeryToken]  //This helps combat Cross-site Request Forgery (CSRF or XSRF) attacks
    public class FormController : Controller
    {
        private DataContext context;

        public FormController(DataContext dbContext)
        {
            context = dbContext;
        }

        public async Task<IActionResult> Index(long id = 1)
        {
            ViewBag.Categories = new SelectList(context.Categories, "CategoryId", "Name");

            return View("Form", await context.Products.Include(p => p.Category)
                                                      .Include(p => p.Supplier)
                                                      .FirstAsync(p => p.ProductId == id)); //adds the nested properties
        }

        #region MyRegion
        //public async Task<IActionResult> Index(long id = 1)
        //{
        //    return View("Form", await context.Products.FindAsync(id));
        //} 
        #endregion

        #region Pre-Model Binding lesson
        //[HttpPost]
        //public IActionResult SubmitForm()
        //{
        //    //foreach (string key in Request.Form.Keys.Where(k => !k.StartsWith("_"))) //This was used just to hide the antiforgerytoken attribute
        //    foreach (string key in Request.Form.Keys)
        //        {
        //        TempData[key] = string.Join(", ", Request.Form[key]);
        //    }
        //    return RedirectToAction(nameof(Results));
        //} 
        #endregion

        #region Model Binding individual properties
        //[HttpPost]
        //public IActionResult SubmitForm(string name, decimal price)
        //{
        //    TempData["name param"] = name;
        //    TempData["price param"] = price.ToString();
        //    return RedirectToAction(nameof(Results));
        //} 
        #endregion

        #region using a complex object for model binding
        //[HttpPost]
        //public IActionResult SubmitForm(Product product)
        //{
        //    TempData["product"] = System.Text.Json.JsonSerializer.Serialize(product);
        //    return RedirectToAction(nameof(Results));
        //} 
        #endregion

        #region Example binding to alternate types of objects
        //[HttpPost]
        ////public IActionResult SubmitForm(Category category)
        //public IActionResult SubmitForm([Bind(Prefix = "Category")] Category category)
        //{
        //    TempData["category"] = System.Text.Json.JsonSerializer.Serialize(category);
        //    return RedirectToAction(nameof(Results));
        //} 
        #endregion

        [HttpPost]
        public IActionResult SubmitForm([Bind("Name", "Category")] Product product) 
            //The Bind attribute specifies exactly which props to bind for Product and excludes any others; this reduces the risk of 'over-binding' attacks
        {
            TempData["name"] = product.Name;
            TempData["price"] = product.Price.ToString();
            TempData["category name"] = product.Category.Name;
            return RedirectToAction(nameof(Results));
        }

        public IActionResult Results()
        {
            return View(TempData);
        }
    }
}
