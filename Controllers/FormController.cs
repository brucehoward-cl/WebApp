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

        [HttpPost]
        public IActionResult SubmitForm()
        {
            //foreach (string key in Request.Form.Keys.Where(k => !k.StartsWith("_"))) //This was used just to hide the antiforgerytoken attribute
            foreach (string key in Request.Form.Keys)
                {
                TempData[key] = string.Join(", ", Request.Form[key]);
            }
            return RedirectToAction(nameof(Results));
        }

        public IActionResult Results()
        {
            return View(TempData);
        }
    }
}
