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
        private readonly IDeletableEntityRepository<Food> foodRepository;

        public SearchService(IDeletableEntityRepository<Workout> workoutsRepository, IDeletableEntityRepository<MealPlan> mealPlansRepository, IDeletableEntityRepository<Meal> mealRepository, IDeletableEntityRepository<Food> foodRepository)
        {
            this.workoutsRepository = workoutsRepository;
            this.mealPlansRepository = mealPlansRepository;
            this.mealRepository = mealRepository;
            this.foodRepository = foodRepository;
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
                Calories = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Calories).Sum(),
                Proteins = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Proteins).Sum(),
                Carbs = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Carbs).Sum(),
                Fats = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Fats).Sum(),
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
                Calories = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Calories).Sum(),
                Proteins = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Proteins).Sum(),
                Carbs = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Carbs).Sum(),
                Fats = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Fats).Sum(),
                Id = x.Id,
            });

            return new PaginatedList<AllMealPLansViewModel>().CreateAsync(mealPlans, pageNumber, 5).GetAwaiter().GetResult();
        }
    }
}