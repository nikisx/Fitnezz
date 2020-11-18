using System.Collections.Generic;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public interface IUsersService
    {
        ApplicationUser GetUser(string username);

        ApplicationUser GetTrainer(string username);

        List<List<AllWourkoutsViewModel>> GetAllUsersWorkout(string userId);
    }
}