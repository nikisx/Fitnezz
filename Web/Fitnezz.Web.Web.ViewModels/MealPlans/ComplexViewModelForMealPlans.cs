using System.Collections.Generic;

namespace Fitnezz.Web.Web.ViewModels.MealPlans
{
    public class ComplexViewModelForMealPlans
    {
        public PaginatedList<AllMealPLansViewModel> ViewModel { get; set; }

        public AddMealPlanInputModel InputModel { get; set; }
    }
}