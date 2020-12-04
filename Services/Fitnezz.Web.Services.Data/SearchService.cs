using System.Linq;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public class SearchService : ISearchService
    {
        private readonly IDeletableEntityRepository<Workout> workoutsRepository;
        private readonly IDeletableEntityRepository<MealPlan> mealPlansRepository;
        private readonly IDeletableEntityRepository<Meal> mealRepository;

        public SearchService(IDeletableEntityRepository<Workout> workoutsRepository, IDeletableEntityRepository<MealPlan> mealPlansRepository, IDeletableEntityRepository<Meal> mealRepository)
        {
            this.workoutsRepository = workoutsRepository;
            this.mealPlansRepository = mealPlansRepository;
            this.mealRepository = mealRepository;
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

        public PaginatedList<AllMealPLansViewModel> SearchMealPlans(string searchWord, int pageNumber)
        {
            var mealPlans = this.mealPlansRepository.All().Where(x => x.Name.Contains(searchWord)).Select(x => new AllMealPLansViewModel()
            {
                Name = x.Name,
                Img = x.Img,
                Calories = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Calories)).ToList(),
                Proteins = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Proteins)).ToList(),
                Carbs = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Carbs)).ToList(),
                Fats = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Fats)).ToList(),
                Id = x.Id,
            });

            return new PaginatedList<AllMealPLansViewModel>().CreateAsync(mealPlans, pageNumber, 5).GetAwaiter().GetResult();
        }

        public PaginatedList<AllMealPLansViewModel> SearchMealPlansPublic(string searchWord, int pageNumber)
        {
            var mealPlans = this.mealPlansRepository.All().Where(x => x.Name.Contains(searchWord) && x.IsPublic).Select(x => new AllMealPLansViewModel()
            {
                Name = x.Name,
                Img = x.Img,
                Calories = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Calories)).ToList(),
                Proteins = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Proteins)).ToList(),
                Carbs = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Carbs)).ToList(),
                Fats = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Fats)).ToList(),
                Id = x.Id,
            });

            return new PaginatedList<AllMealPLansViewModel>().CreateAsync(mealPlans, pageNumber, 5).GetAwaiter().GetResult();
        }
    }
}