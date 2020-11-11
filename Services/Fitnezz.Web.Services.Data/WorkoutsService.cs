using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public class WorkoutsService : IWorkoutsService
    {
        private readonly IDeletableEntityRepository<Workout> workoutsRepository;

        public WorkoutsService(IDeletableEntityRepository<Workout> workoutsRepository)
        {
            this.workoutsRepository = workoutsRepository;
        }

        public IEnumerable<AllWourkoutsViewModel> GetAll()
        {
            return this.workoutsRepository.All().Select(x=> new AllWourkoutsViewModel
            {
                Name = x.Name,
                ExercisesCount = x.Exercises.Count,
                Id = x.Id,
            }).ToList();
        }

        public async Task Create(string name)
        {
            var workout = new Workout()
            {
                Name = name,
            };

            await this.workoutsRepository.AddAsync(workout);
            await this.workoutsRepository.SaveChangesAsync();
        }

        public string GetWorkoutName(int id)
        {
            return this.workoutsRepository.All().Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
        }

        public DetailsWorkoutsVIewModel GetWorkoutDetails(int id)
        {
            return this.workoutsRepository.All().Where(x => x.Id == id).Select(x => new DetailsWorkoutsVIewModel
            {
                Name = x.Name,
                Id = x.Id,
                Exercises = x.Exercises,
            }).FirstOrDefault();
        }
    }
}