using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class MealPlansController : Controller
    {
        private readonly IMealPlansService mealPlansService;

        public MealPlansController(IMealPlansService mealPlansService)
        {
            this.mealPlansService = mealPlansService;
        }

        public IActionResult All()
        {
          var viewModel = new ComplexViewModelForMealPlans()
          {
              InputModel = new AddMealPlanInputModel(),

              ViewModel = this.mealPlansService.GetAll(),
          };

          return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var viewModel = this.mealPlansService.GetDetails(id);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddMealPlanInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData["Message"] = this.ModelState.Values.SelectMany(modelState => modelState.Errors).FirstOrDefault().ErrorMessage;
                return RedirectToAction("All");
            }

            await this.mealPlansService.CreateMealPLan(input);

            return RedirectToAction("All");
        }

        public IActionResult AddMealPlanToUser(string username, int mealPlanId)
        {
            return this.RedirectToAction("All");
        }

        public IActionResult CreateFood(int mealId)
        {
            this.ViewBag.MealName = this.mealPlansService.GetMealName(mealId);
            var input = new AddFoodInputModel();
            //maybe a food controller
            return this.View(input);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(AddFoodInputModel input)
        {
            

            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            //maybe a food controller
            await this.mealPlansService.CreateFood(input);
            return RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeal(string mealName, int mealPlanId)
        {
            await this.mealPlansService.CreateMeal(mealName, mealPlanId);
            //maybe a food controller
            return Redirect($"/MealPlans/Details?id={mealPlanId}");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.mealPlansService.DeleteMealPLan(id);
            return RedirectToAction("All");
        }

        public async Task<IActionResult> DeleteFood(int mealPlanId, int foodId)
        {
            await this.mealPlansService.DeleteFood(foodId);

            return this.Redirect($"/MealPlans/Details?id={mealPlanId}");
        }
    }
}
