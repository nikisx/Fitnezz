using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels.MealPlans;

namespace Fitnezz.Web.Services.Data
{
    public interface IMealPlansService
    {
        IEnumerable<AllMealPLansViewModel> GetAll();

        Task CreateMealPLan(AddMealPlanInputModel input);

        Task DeleteMealPLan(int id);

        MealPlanDetailsViewModel GetDetails(int id);

        Task CreateMeal(string mealName, int mealPlanId);

        Task CreateFood(AddFoodInputModel input);
    }
}