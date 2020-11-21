using System.Collections.Generic;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels.Trainers;

namespace Fitnezz.Web.Services.Data
{
    public interface ITrainersService
    {
        Task Create(TrainerCreateInputModel input);

        IEnumerable<AllTrainersViewModel> GetAll();

        Task GetHired(string trainerId, string userId);

        IEnumerable<AllClientsViewModel> GetClients(string trainerId);

        Task DeleteUsersWorkout(string userId, int workoutId);
    }
}