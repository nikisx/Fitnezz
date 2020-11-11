using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class Food : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public int Grams { get; set; }

        public int Calories { get; set; }

        public int MealId { get; set; }

        public Meal Meal { get; set; }

        public int Proteins { get; set; }

        public int Carbs { get; set; }

        public int Fats { get; set; }
    }
}