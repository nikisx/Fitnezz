using System.Collections.Generic;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.Classes;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public interface IUsersService
    {
        ApplicationUser GetUserByUserName(string username);

        ApplicationUser GetTrainer(string username);

        List<List<AllWourkoutsViewModel>> GetAllUsersWorkout(string userId);

        bool UserHasWorkout(string userId, int workoutId);

        ApplicationUser GetUserById(string id);

        List<List<AllMealPLansViewModel>> GetUserMealPlans(string userId);

        Task UpdateProfile(ProfileUpdateInputModel input, string userId);

    }
}