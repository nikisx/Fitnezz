using System;
using System.Collections.Generic;
using System.Text;

namespace Fitnezz.Web.Web.ViewModels.MealPlans
{
   public class AllMealPLansViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Proteins { get; set; }

        public int Calories { get; set; }

        public int Carbs { get; set; }

        public int Fats { get; set; }

        public string Img { get; set; }
    }
}
