using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ContentController : ControllerBase
    {
        private DataContext context;
        public ContentController(DataContext dataContext)
        {
            context = dataContext;
        }

        [HttpGet("string")]
        public string GetString() => "This is a string response";

        #region Prior to Content Negotiation (XML Formatting)
        //[HttpGet("object")]
        //public async Task<Product> GetObject()
        //{
        //    return await context.Products.FirstAsync();
        //} 
        #endregion

        #region Prior to Requesting a Format in the URL
        //[HttpGet("object")]
        //[Produces("application/json")]   //constrains the result 
        #endregion
        [HttpGet("object/{format?}")]
        [FormatFilter]  //allows the URL format request to override the header's Accept request
        [Produces("application/json", "application/xml")]
        public async Task<ProductBindingTarget> GetObject()
        {
            Product p = await context.Products.FirstAsync();
            return new ProductBindingTarget()
            {
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                SupplierId = p.SupplierId
            };
        }

        [HttpPost]
        [Consumes("application/json")]  //restricts the data type this method will handle
        public string SaveProductJson(ProductBindingTarget product)
        {
            return $"JSON: {product.Name}";
        }

        [HttpPost]
        [Consumes("application/xml")]   //restricts the data type this method will handle
        public string SaveProductXml(ProductBindingTarget product)
        {
            return $"XML: {product.Name}";
        }
    }
}
