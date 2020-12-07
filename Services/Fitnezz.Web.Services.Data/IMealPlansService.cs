using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.MealPlans;

namespace Fitnezz.Web.Services.Data
{
    public interface IMealPlansService
    {
        PaginatedList<AllMealPLansViewModel> GetAll(int pageNumber);

        Task CreateMealPLan(AddMealPlanInputModel input);

        Task DeleteMealPLan(int id);

        MealPlanDetailsViewModel GetDetails(int id);

        Task CreateMeal(string mealName, int mealPlanId);

        Task CreateFood(AddFoodInputModel input);

        Task DeleteFood(int id);

        string GetMealName(int id);

        Task AddMealPlanToUser(string userId, int mealPlanId);

        bool UserHasMealPlan(string userId, int mealPlanId);

        PaginatedList<AllMealPLansViewModel> GetAllPublic(int pageNumber);

        Task DeleteMeal(int mealId);
    }
}