using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public interface ISearchService
    {
        PaginatedList<AllWourkoutsViewModel> SearchWorkouts(string searchWord, int pageNumber);

        PaginatedList<AllWourkoutsViewModel> SearchWorkoutsPublic(string searchWord, int pageNumber);

        PaginatedList<AllMealPLansViewModel> SearchMealPlans(string searchWord, int pageNumber);

        PaginatedList<AllMealPLansViewModel> SearchMealPlansPublic(string searchWord, int pageNumber);
    }
}