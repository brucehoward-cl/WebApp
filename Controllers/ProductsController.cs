using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private DataContext context;
        public ProductsController(DataContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IAsyncEnumerable<Product> GetProducts()
        {
            return context.Products;
        }

        #region GetProduct
        //[HttpGet("{id}")]
        //public async Task<Product> GetProduct(long id)
        //{
        //    return await context.Products.FindAsync(id);
        //} 
        #endregion

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            Product p = await context.Products.FindAsync(id);
            if (p == null)
            {
                return NotFound();
            }
            //return Ok(p);

            //This added to show how to omit foreign relationship properties (like supplier and category)
            return Ok(new
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                SupplierId = p.SupplierId
            });
        }
        #region GetProduct
        //[HttpGet("{id}")]
        //public Product GetProduct(long id, [FromServices] ILogger<ProductsController> logger)
        //{
        //    logger.LogDebug("GETPRODUCT ACTION INVOKED!!!!!!!!!!!!!!!!!!");
        //    //return context.Products.FirstOrDefault();
        //    return context.Products.Find(id);
        //} 
        #endregion

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductBindingTarget target)
        {
            Product p = target.ToProduct();
            await context.Products.AddAsync(p);
            await context.SaveChangesAsync();
            return Ok(p);
        }

        #region SaveProduct
        //This method before the [API Controller] attribute added
        //[HttpPost]
        //public async Task<IActionResult> SaveProduct([FromBody] ProductBindingTarget target)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Product p = target.ToProduct();
        //        await context.Products.AddAsync(p);
        //        await context.SaveChangesAsync();
        //        return Ok(p);
        //    }
        //    return BadRequest(ModelState);
        //} 
        #endregion

        #region SaveProduct
        //[HttpPost]
        //public async Task SaveProduct([FromBody] ProductBindingTarget target)
        //{
        //    await context.Products.AddAsync(target.ToProduct());
        //    await context.SaveChangesAsync();
        //} 
        #endregion

        #region SaveProduct
        //[HttpPost]
        //public void SaveProduct([FromBody] Product product)
        //{
        //    context.Products.Add(product);
        //    context.SaveChanges();
        //} 
        #endregion

        [HttpPut]
        public async Task UpdateProduct(Product product)
        {
            context.Update(product);
            await context.SaveChangesAsync();
        }

        #region UpdateProduct
        //This method before the [API Controller] attribute added
        //[HttpPut]
        //public async Task UpdateProduct([FromBody] Product product)
        //{
        //    context.Update(product);
        //    await context.SaveChangesAsync();
        //} 
        #endregion

        #region UpdateProduct
        //[HttpPut]
        //public void UpdateProduct([FromBody] Product product)
        //{
        //    context.Products.Update(product);
        //    context.SaveChanges();
        //} 
        #endregion

        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id)
        {
            context.Products.Remove(new Product() { ProductId = id });
            await context.SaveChangesAsync();
        }

        #region DeleteProduct
        //[HttpDelete("{id}")]
        //public void DeleteProduct(long id)
        //{
        //    context.Products.Remove(new Product() { ProductId = id });
        //    context.SaveChanges();
        //} 
        #endregion

        [HttpGet("redirect")]
        public IActionResult Redirect()
        {
            //return Redirect("/api/products/1");
            return RedirectToAction(nameof(GetProduct), new { Id = 1 });
        }
    }
}