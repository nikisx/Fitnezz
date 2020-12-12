using Fitnezz.Web.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Fitnezz.Web.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Fitnezz.Web.Data.Common.Repositories;
    using Fitnezz.Web.Data.Models;
    using Fitnezz.Web.Web.ViewModels.MealPlans;

    public class MealPlansService : IMealPlansService
    {
        private readonly IDeletableEntityRepository<MealPlan> mealPlanRepository;
        private readonly IDeletableEntityRepository<Meal> mealRepository;
        private readonly IDeletableEntityRepository<Food> foodRepository;
        private readonly IDeletableEntityRepository<TraineesMealPlans> traineeMealPlanRepository;

        public MealPlansService(IDeletableEntityRepository<MealPlan> mealPlanRepository, IDeletableEntityRepository<Meal> mealRepository, IDeletableEntityRepository<Food> foodRepository, IDeletableEntityRepository<TraineesMealPlans> traineeMealPlanRepository)
        {
            this.mealPlanRepository = mealPlanRepository;
            this.mealRepository = mealRepository;
            this.foodRepository = foodRepository;
            this.traineeMealPlanRepository = traineeMealPlanRepository;
        }

        public PaginatedList<AllMealPLansViewModel> GetAll(int pageNumber)
        {
            // var calories = this.mealRepository.All().Where(a => a.MealPlanId == 3).Select(c => c.Foods.Sum(f=>f.Calories)).ToList();
            var mealPlans = this.mealPlanRepository.All().OrderByDescending(x=>x.CreatedOn).Select(x=> new AllMealPLansViewModel
            {
                Name = x.Name,
                Img = x.Img,
                Calories = this.foodRepository.All().Where(f=> f.Meal.MealPlanId == x.Id).Select(c=> c.Calories).Sum(),
                Proteins = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Proteins).Sum(),
                Carbs = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Carbs).Sum(),
                Fats = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Fats).Sum(),
                Id = x.Id,
            });

            var paginatedList = new PaginatedList<AllMealPLansViewModel>().CreateAsync(mealPlans, pageNumber, 4).GetAwaiter().GetResult();

            return paginatedList;
        }

        public async Task CreateMealPLan(AddMealPlanInputModel input)
        {
            var mealPlan = new MealPlan
            {
                Name = input.MealPlanName,
                Img = input.Img,
                IsPublic = input.IsPublic == PublicType.Public.ToString(),
            };

            await this.mealPlanRepository.AddAsync(mealPlan);
            await this.mealPlanRepository.SaveChangesAsync();
        }

        public async Task DeleteMealPLan(int id)
        {
            var mealPlan = this.mealPlanRepository.All().FirstOrDefault(x => x.Id == id);

            var userMealPLans = this.traineeMealPlanRepository.All().Where(x=>x.MealPlanId == id);

            foreach (var userMealPLan in userMealPLans)
            {
                this.traineeMealPlanRepository.HardDelete(userMealPLan);
            }

            this.mealPlanRepository.Delete(mealPlan);
            await this.mealPlanRepository.SaveChangesAsync();
        }

        public MealPlanDetailsViewModel GetDetails(int id)
        {
            return this.mealPlanRepository.All().Where(x => x.Id == id).Select(x => new MealPlanDetailsViewModel
            {
                IsPublic = x.IsPublic,
                Name = x.Name,
                Id = x.Id,
                Meals = x.Meals.Select(a => new MealsViewModel
                {
                    Name = a.Name,
                    Id = a.Id,
                    Foods = a.Foods.Select(f => new FoodViewModel
                    {
                        Name = f.Name,
                        Calories = f.Calories,
                        Grams = f.Grams,
                        Proteins = f.Proteins,
                        Carbs = f.Carbs,
                        Fats = f.Fats,
                        Id = f.Id,
                    }).ToList(),
                }).ToList(),
            }).FirstOrDefault();
        }

        public async Task CreateMeal(string mealName, int mealPlanId)
        {
            var meal = new Meal()
            {
                Name = mealName,
                MealPlanId = mealPlanId,
            };

            await this.mealRepository.AddAsync(meal);
            await this.mealRepository.SaveChangesAsync();
        }

        public async Task CreateFood(AddFoodInputModel input)
        {
            var food = new Food()
            {
                Name = input.Name,
                Id = input.Id,
                Calories = input.Calories,
                Grams = input.Grams,
                Proteins = input.Proteins,
                Carbs = input.Carbs,
                Fats = input.Fats,
                MealId = input.MealId,
            };

            await this.foodRepository.AddAsync(food);
            await this.foodRepository.SaveChangesAsync();
        }

        public async Task DeleteFood(int id)
        {
            var food = this.foodRepository.All().FirstOrDefault(x => x.Id == id);

            this.foodRepository.Delete(food);
            await this.foodRepository.SaveChangesAsync();
        }

        public string GetMealName(int id)
        {
            return this.mealRepository.All().Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
        }

        public async Task AddMealPlanToUser(string userId, int mealPlanId)
        {
            var mealPlanToUser = new TraineesMealPlans()
            {
                TraineeId = userId,
                MealPlanId = mealPlanId,
            };

            await this.traineeMealPlanRepository.AddAsync(mealPlanToUser);
            await this.traineeMealPlanRepository.SaveChangesAsync();
        }

        public bool UserHasMealPlan(string userId, int mealPlanId)
        {
            return this.traineeMealPlanRepository.All().Any(x => x.MealPlanId == mealPlanId && x.TraineeId == userId);
        }

        public PaginatedList<AllMealPLansViewModel> GetAllPublic(int pageNumber)
        {
            var mealPlans = this.mealPlanRepository.All().Where(x=>x.IsPublic == true).OrderByDescending(x => x.CreatedOn).Select(x => new AllMealPLansViewModel
            {
                Name = x.Name,
                Img = x.Img,
                Calories = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Calories).Sum(),
                Proteins = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Proteins).Sum(),
                Carbs = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Carbs).Sum(),
                Fats = this.foodRepository.All().Where(f => f.Meal.MealPlanId == x.Id).Select(c => c.Fats).Sum(),
                Id = x.Id,
            });

            var paginatedList = new PaginatedList<AllMealPLansViewModel>().CreateAsync(mealPlans, pageNumber, 6).GetAwaiter().GetResult();

            return paginatedList;
        }

        public async Task DeleteMeal(int mealId)
        {
            var meal = this.mealRepository.All().FirstOrDefault(x => x.Id == mealId);

            this.mealRepository.Delete(meal);
            await this.mealRepository.SaveChangesAsync();
        }
    }
}