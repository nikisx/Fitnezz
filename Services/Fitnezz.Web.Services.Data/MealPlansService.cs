using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.MealPlans;

namespace Fitnezz.Web.Services.Data
{
    public class MealPlansService : IMealPlansService
    {
        private readonly IDeletableEntityRepository<MealPlan> mealPlanRepository;

        public MealPlansService(IDeletableEntityRepository<MealPlan> mealPlanRepository)
        {
            this.mealPlanRepository = mealPlanRepository;
        }

        public IEnumerable<AllMealPLansViewModel> GetAll()
        {
            return this.mealPlanRepository.All().Select(x=> new AllMealPLansViewModel
            {
                Name = x.Name,
                Calories = x.Calories,
                Proteins = x.Proteins,
                Carbs = x.Carbs,
                Fats = x.Fats,
                Img = x.Img,
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
            var mealPlan = mealPlanRepository.All().FirstOrDefault(x => x.Id == id);

            this.mealPlanRepository.Delete(mealPlan);
            await this.mealPlanRepository.SaveChangesAsync();
        }
    }
}