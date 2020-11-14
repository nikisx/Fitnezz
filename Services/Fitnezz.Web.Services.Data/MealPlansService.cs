﻿namespace Fitnezz.Web.Services.Data
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

        public MealPlansService(IDeletableEntityRepository<MealPlan> mealPlanRepository, IDeletableEntityRepository<Meal> mealRepository, IDeletableEntityRepository<Food> foodRepository)
        {
            this.mealPlanRepository = mealPlanRepository;
            this.mealRepository = mealRepository;
            this.foodRepository = foodRepository;
        }

        public IEnumerable<AllMealPLansViewModel> GetAll()
        {
            var calories = this.mealRepository.All().Where(a => a.MealPlanId == 3).Select(c => c.Foods.Sum(f=>f.Calories)).ToList();

            return this.mealPlanRepository.All().OrderByDescending(x=>x.CreatedOn).Select(x=> new AllMealPLansViewModel
            {
                Name = x.Name,
                Img = x.Img,
                Calories = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Calories)).ToList(),
                Proteins = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Proteins)).ToList(),
                Carbs = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Carbs)).ToList(),
                Fats = this.mealRepository.All().Where(a => a.MealPlanId == x.Id).Select(c => c.Foods.Sum(f => f.Fats)).ToList(),
                Id = x.Id,
            }).ToList();
        }

        public async Task CreateMealPLan(AddMealPlanInputModel input)
        {
            var mealPlan = new MealPlan
            {
                Name = input.MealPlanName,
                Img = input.Img,
            };

            await this.mealPlanRepository.AddAsync(mealPlan);
            await this.mealPlanRepository.SaveChangesAsync();
        }

        public async Task DeleteMealPLan(int id)
        {
            var mealPlan = this.mealPlanRepository.All().FirstOrDefault(x => x.Id == id);

            this.mealPlanRepository.Delete(mealPlan);
            await this.mealPlanRepository.SaveChangesAsync();
        }

        public MealPlanDetailsViewModel GetDetails(int id)
        {
            return this.mealPlanRepository.All().Where(x => x.Id == id).Select(x => new MealPlanDetailsViewModel
            {
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
    }
}