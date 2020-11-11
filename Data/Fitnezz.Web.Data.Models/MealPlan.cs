using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class MealPlan : BaseDeletableModel<int>
    {
        public MealPlan()
        {
            this.Meals = new HashSet<Meal>();
            
        }

        public int Calories => this.Meals.Sum(x => x.Calories);

        public string Name { get; set; }

        public ICollection<Meal> Meals { get; set; }

        public int Proteins => this.Meals.Sum(x => x.Proteins);

        public int Carbs => this.Meals.Sum(x => x.Carbs);

        public int Fats => this.Meals.Sum(x => x.Fats);

        public string Img { get; set; }
    }
}
