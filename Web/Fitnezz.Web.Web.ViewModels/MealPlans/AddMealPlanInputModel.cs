﻿using System.ComponentModel.DataAnnotations;

namespace Fitnezz.Web.Web.ViewModels.MealPlans
{
    public class AddMealPlanInputModel
    {
        [MinLength(5,ErrorMessage = "Name should be minimum of 5 letter!")]
        [MaxLength(50, ErrorMessage = "Name should be maximum of 50 letter!")]
        [Required(ErrorMessage = "Meal plan name is required")]
        public string MealPlanName { get; set; }

        [Required]
        [Url]
        public string Img { get; set; }

        public string IsPublic { get; set; }
    }
}