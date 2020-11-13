using System.Collections.Generic;
using System.Linq;

namespace Fitnezz.Web.Web.ViewModels.MealPlans
{

    public class MealsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Calories => this.Foods.Sum(x => x.Calories);

        public int Proteins => this.Foods.Sum(x => x.Proteins);

        public int Carbs => this.Foods.Sum(x => x.Carbs);

        public int Fats => this.Foods.Sum(x => x.Fats);

        public ICollection<FoodViewModel> Foods { get; set; }
    }
}