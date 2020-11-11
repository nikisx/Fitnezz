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
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddMealPlanInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("All");
            }

            await this.mealPlansService.CreateMealPLan(input);

            return RedirectToAction("All");
        }

        public IActionResult AddMealPlanToUser(string username, int mealPlanId)
        {
            return this.RedirectToAction("All");
        }

        public IActionResult CreateFood(string mealId)
        {
            this.ViewBag.Name = mealId;
            var input = new AddFoodInputModel();
            //maybe a food controller
            return this.View(input);
        }

        [HttpPost]
        public IActionResult CreateFood(AddFoodInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.View(input);
            }
            //maybe a food controller
            return RedirectToAction("All");
        }

        [HttpPost]
        public IActionResult CreateMeal(string mealName, string mealPlanId)
        {
            //maybe a food controller
            return RedirectToAction("All");
        }
    }
}
