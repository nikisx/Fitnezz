using Fitnezz.Web.Common;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;

namespace Fitnezz.Web.Web.Controllers
{
    using System.Diagnostics;

    using Fitnezz.Web.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IUsersService usersService;

        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult CalorieCalculator()
        {
            var inputModel = new CalorieCalculatorInputModel();

            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                inputModel.WeightAvailable = false;
            }
            else
            {
                inputModel.WeightAvailable = true;
            }

            return this.View(inputModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CalorieCalculator(CalorieCalculatorInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            CalculatedCaloriesViewModel viewModel = new CalculatedCaloriesViewModel();

            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                var user = this.usersService.GetUserByUserName(this.User.Identity.Name);
                viewModel.Calories = this.usersService.CalculateCalories(input.Goal, user.Weight);
                viewModel.Goal = input.Goal.ToString();
            }
            else
            {
                var userWeight = this.usersService.GetTrainer(this.User.Identity.Name).Weight;
                viewModel.Calories = this.usersService.CalculateCalories(input.Goal, userWeight);
                viewModel.Goal = input.Goal.ToString();
            }

            return this.View("CalculatedCalories", viewModel);
        }

        public IActionResult CalculatedCalories(CalculatedCaloriesViewModel viewModel)
        {
            return this.View(viewModel);
        }

        public IActionResult StatusCodeError(int statusCode)
        {
            if (statusCode == 404)
            {
                return this.View();
            }

            return this.Redirect("Error");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
