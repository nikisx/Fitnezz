using System.Collections.Generic;

namespace Fitnezz.Web.Web.ViewModels.MealPlans
{
    public class MealPlanDetailsViewModel
    {
        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public int Id { get; set; }

        public ICollection<MealsViewModel> Meals { get; set; }
    }
}