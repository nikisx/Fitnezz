using System.Collections.Generic;
using System.Linq;
using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class Meal : BaseDeletableModel<int>
    {
        public Meal()
        {
            this.Foods = new HashSet<Food>();
        }

        public string Name { get; set; }

        public int Calories => this.Foods.Sum(x => x.Calories);

        public int MealPlanId { get; set; }

        public MealPlan MealPlan { get; set; }

        public int Proteins => this.Foods.Sum(x => x.Proteins);

        public int Carbs => this.Foods.Sum(x => x.Carbs);

        public int Fats => this.Foods.Sum(x => x.Fats);

        public ICollection<Food> Foods { get; set; }

    }
}