using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddCheeseViewModel
    {
        [Required]
        [Display(Name = "Cheese Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must give your cheese a description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        public List<SelectListItem> Categories { get; set; }

        // Default constructor needed so model binding will work
        public AddCheeseViewModel() { }

                                  // Categories given when controller creates this object.
        public AddCheeseViewModel(IEnumerable<CheeseCategory> categories) {

            Categories = new List<SelectListItem>();

            foreach (CheeseCategory category in categories)
            {
                Categories.Add(new SelectListItem
                {
                    Value = category.ID.ToString(),
                    Text = category.Name
                });
            }

        }
    }
}
