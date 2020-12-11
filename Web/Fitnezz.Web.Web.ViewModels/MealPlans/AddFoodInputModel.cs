using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitnezz.Web.Web.ViewModels.MealPlans
{
    public class AddFoodInputModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Range(1, 2000)]
        [Required]
        public int Calories { get; set; }

        [Range(10, 1000)]
        [Required]
        public int Grams { get; set; }

        [Range(0, 100)]
        [Required]
        public int Proteins { get; set; }

        [Range(0, 200)]
        [Required]
        public int Carbs { get; set; }

        [Required]
        [Range(0, 200)]
        public int Fats { get; set; }

        public int MealId { get; set; }
    }
}
