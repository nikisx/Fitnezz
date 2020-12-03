using System.Linq;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public class SearchService : ISearchService
    {
        private readonly IDeletableEntityRepository<Workout> workoutsRepository;

        public SearchService(IDeletableEntityRepository<Workout> workoutsRepository)
        {
            this.workoutsRepository = workoutsRepository;
        }

        public PaginatedList<AllWourkoutsViewModel> SearchWorkouts(string searchWord, int pageNumber)
        {
            var workouts = this.workoutsRepository.All().Where(x => x.Name.Contains(searchWord)).Select(x=> new AllWourkoutsViewModel()
            {
                Name = x.Name,
                ExercisesCount = x.Exercises.Count,
                Id = x.Id,
            });

            return new PaginatedList<AllWourkoutsViewModel>().CreateAsync(workouts, pageNumber, 5).GetAwaiter().GetResult();
        }

        public PaginatedList<AllWourkoutsViewModel> SearchWorkoutsPublic(string searchWord, int pageNumber)
        {
            var workouts = this.workoutsRepository.All().Where(x => x.Name.Contains(searchWord) && x.IsPublic == true).Select(x => new AllWourkoutsViewModel()
            {
                Name = x.Name,
                ExercisesCount = x.Exercises.Count,
                Id = x.Id,
            });

            return new PaginatedList<AllWourkoutsViewModel>().CreateAsync(workouts, pageNumber, 5).GetAwaiter().GetResult();
        }
    }
}