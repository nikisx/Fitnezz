using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public interface IWorkoutsService
    {
        PaginatedList<AllWourkoutsViewModel> GetAll(int pageNumber);

        Task Create(string name, string isPublic);

        string GetWorkoutName(int id);

        DetailsWorkoutsVIewModel GetWorkoutDetails(int id);

        Task CreateExercise(AddExerciseToWorkoutInputModel input);

        Task DeleteWorkout(int id);

        Task AddWorkoutToUserAsync(string userId, int workoutId);

        PaginatedList<AllWourkoutsViewModel> GetAllPublic(int pageNumber);

        PaginatedList<AllWourkoutsViewModel> GetAllWithDeletedWorkouts(int pageNumber);

        Task RestoreWorkout(int id);
    }
}