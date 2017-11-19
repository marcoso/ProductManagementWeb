using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProductManagementWebSample.Models
{
    public class ProductModel
    {
        [Required]        
        [Display(Name = "Product Number")]        
        [RegularExpression("[0-9]+", ErrorMessage = "Please provide a correct product number")]
        public string ProductNumber { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Please provide a correct title, 5 characters minimum length.")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Price")]
        [RegularExpression("[0-9]+", ErrorMessage = "Please provide a correct price")]
        public string Price { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public string DateCreated { get; set; }
    }
}