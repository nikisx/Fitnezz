using System.Collections.Generic;

namespace Fitnezz.Web.Web.ViewModels.MealPlans
{
    public class ComplexViewModelForMealPlans
    {
        public IEnumerable<AllMealPLansViewModel> ViewModel { get; set; }

        public AddMealPlanInputModel InputModel { get; set; }
    }
}