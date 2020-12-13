using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fitnezz.Web.Data.Models;

namespace Fitnezz.Web.Web.ViewModels.MealPlans
{
   public class AllMealPLansViewModel
    {
        public AllMealPLansViewModel()
        {
            //this.Proteins=new List<int>();
            //this.Carbs = new List<int>();
            ////this.Calories = new List<int>();
            //this.Fats = new List<int>();
        }

        public bool IsDeleted { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Proteins { get; set; }

        public int Calories { get; set; }

        public int Carbs { get; set; }

        public int Fats { get; set; }

        public string Img { get; set; }

        public string UserId { get; set; }

    }
}
