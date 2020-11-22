using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class MealPlansController : Controller
    {
        private readonly IMealPlansService mealPlansService;
        private readonly IUsersService usersService;

        public MealPlansController(IMealPlansService mealPlansService, IUsersService usersService)
        {
            this.mealPlansService = mealPlansService;
            this.usersService = usersService;
        }

        public IActionResult All(int pageNumber = 1)
        {
          var viewModel = new ComplexViewModelForMealPlans()
          {
              InputModel = new AddMealPlanInputModel(),

              ViewModel = this.mealPlansService.GetAll(pageNumber),
          };

          return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var viewModel = this.mealPlansService.GetDetails(id);

            if (!viewModel.IsPublic)
            {
                if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) || !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
                {
                    var userId = this.usersService.GetUserByUserName(this.User.Identity.Name).Id;
                    if (!this.mealPlansService.UserHasMealPlan(userId, id))
                    {
                        return this.RedirectToAction("All");
                    }
                }
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddMealPlanInputModel input)
        {
            var viewModel = new ComplexViewModelForMealPlans()
            {
                InputModel = input,
                ViewModel = this.mealPlansService.GetAll(1),
            };

            if (!this.ModelState.IsValid)
            {
                this.TempData["sErrMsg"] = this.ModelState.Values.SelectMany(modelState => modelState.Errors).FirstOrDefault().ErrorMessage;
                return this.View("All", viewModel);
            }

            await this.mealPlansService.CreateMealPLan(input);

            return RedirectToAction("All");
        }

        public async Task<IActionResult> AddMealPlanToUser(string username, int mealPlanId)
        {
            var user = this.usersService.GetUserByUserName(username);
            var trainer = this.usersService.GetTrainer(this.User.Identity.Name);

            if (user == null)
            {
                this.TempData["sErrMsg"] = "User not Found";
                return this.View("All", new ComplexViewModelForMealPlans()
                {
                    ViewModel = this.mealPlansService.GetAll(1),
                    InputModel = new AddMealPlanInputModel(),
                });
            }

            if (trainer.Clients.All(x => x.UserName != username))
            {
                this.TempData["sErrMsg"] = "This trainee is not yours ";
                return this.View("All", new ComplexViewModelForMealPlans()
                {
                    ViewModel = this.mealPlansService.GetAll(1),
                    InputModel = new AddMealPlanInputModel(),
                });
            }

            if (this.mealPlansService.UserHasMealPlan(user.Id, mealPlanId))
            {
                this.TempData["sErrMsg"] = "This trainee already has this meal plan ";
                return this.View("All", new ComplexViewModelForMealPlans()
                {
                    ViewModel = this.mealPlansService.GetAll(1),
                    InputModel = new AddMealPlanInputModel(),
                });
            }

            await this.mealPlansService.AddMealPlanToUser(user.Id, mealPlanId);

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
            if ( mealName == null || mealName.Length < 6 || mealName.Length > 40 || string.IsNullOrWhiteSpace(mealName.TrimEnd()))
            {
                return Redirect($"/MealPlans/Details?id={mealPlanId}");
            }
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

        public PartialViewResult ShowError(string sErrorMessage)
        {
            return this.PartialView("_ErrorPopup");
        }
    }
}
