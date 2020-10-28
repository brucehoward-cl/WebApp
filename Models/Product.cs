using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApp.Validation;
using Microsoft.AspNetCore.Mvc; //added for remote validation

namespace WebApp.Models
{
    //[PhraseAndPrice(Phrase = "Small", Price = "100")]  //custom model attribute applied at a class level //Pre Chapter 31
    public class Product
    {
        public long ProductId { get; set; }

        [Required]  //using this method of model validation ensures all actions use consistent validation logic
        [Display(Name = "Name")]  
        public string Name { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        //[DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)] //used for FormTagHelper lesson, but commented for Model Binding
        //[BindNever]     //This prevents model binding throughout the application; used to reduce 'over-binding' attacks
        [Required(ErrorMessage = "Please enter a price")] //using this method of model validation ensures all actions use consistent validation logic
        [Range(1, 999999, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }

        [PrimaryKey(ContextType = typeof(DataContext), DataType = typeof(Category))]
        //[Remote("CategoryKey", "Validation", ErrorMessage = "Enter an existing key")]   //remote validation
        public long CategoryId { get; set; }
        public Category Category { get; set; }

        [PrimaryKey(ContextType = typeof(DataContext), DataType = typeof(Supplier))]
        //[Remote("SupplierKey", "Validation", ErrorMessage = "Enter an existing key")]   //remote validation
        public long SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}