using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;


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

        public async Task<IActionResult> Index(long? id)
        {
            return View("Form", await context.Products.FirstOrDefaultAsync(p => id == null || p.ProductId == id));
        }

        #region Used prior to adding custom attribute model validation
        //[HttpPost]
        //public IActionResult SubmitForm(Product product)
        //{
        //    #region Used without model property attribute validation
        //    //if (string.IsNullOrEmpty(product.Name))
        //    //{
        //    //    ModelState.AddModelError(nameof(Product.Name), "Enter a name");
        //    //} // commented out after adding attribute validation to the model properties

        //    //if (ModelState.GetValidationState(nameof(Product.Price)) == ModelValidationState.Valid && product.Price < 1)
        //    //{
        //    //    ModelState.AddModelError(nameof(Product.Price), "Enter a positive price");
        //    //}   // commented out after adding attribute validation to the model properties 
        //    #endregion

        //    if (ModelState.GetValidationState(nameof(Product.Name)) == ModelValidationState.Valid 
        //        && ModelState.GetValidationState(nameof(Product.Price)) == ModelValidationState.Valid 
        //        && product.Name.ToLower().StartsWith("small") && product.Price > 100)
        //    {
        //        ModelState.AddModelError("", "Small products cannot cost more than $100"); //Adds a summary level error when a combination of elements is invalid
        //    }

        //    if (!context.Categories.Any(c => c.CategoryId == product.CategoryId))
        //    {
        //        ModelState.AddModelError(nameof(Product.CategoryId), "Enter an existing category ID");
        //    }

        //    if (!context.Suppliers.Any(s => s.SupplierId == product.SupplierId))
        //    {
        //        ModelState.AddModelError(nameof(Product.SupplierId), "Enter an existing supplier ID");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        TempData["name"] = product.Name;
        //        TempData["price"] = product.Price.ToString();
        //        TempData["categoryId"] = product.CategoryId.ToString();
        //        TempData["supplierId"] = product.SupplierId.ToString();
        //        return RedirectToAction(nameof(Results));
        //    }
        //    else
        //    {
        //        return View("Form");
        //    }
        //} 
        #endregion

        [HttpPost]
        public IActionResult SubmitForm(Product product)    //used with custom attribute model validation
        {
            if (ModelState.IsValid)
            {
                TempData["name"] = product.Name;
                TempData["price"] = product.Price.ToString();
                TempData["categoryId"] = product.CategoryId.ToString();
                TempData["supplierId"] = product.SupplierId.ToString();
                return RedirectToAction(nameof(Results));
            }
            else
            {
                return View("Form");
            }
        }

        public IActionResult Results()
        {
            return View(TempData);
        }
    }
}
