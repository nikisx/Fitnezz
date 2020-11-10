using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public interface IWorkoutsService
    {
        IEnumerable<AllWourkoutsViewModel> GetAll();

        Task Create(string name);

        string GetWorkoutName(int id);
    }
}