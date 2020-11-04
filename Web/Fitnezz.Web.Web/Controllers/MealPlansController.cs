using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class MealPlansController : Controller
    {
        public IActionResult All()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return this.View();
        }
        [HttpPost]
        public IActionResult Create(string mealPlanName)
        {
            return RedirectToAction("All");
        }

        public IActionResult AddMealPlanToUser(string username, int mealPlanId)
        {
            return this.RedirectToAction("All");
        }

        public IActionResult CreateFood(string mealId)
        {
            this.ViewBag.Name = mealId;
            //maybe a food controller
            return this.View();
        }

        [HttpPost]
        public IActionResult CreateFood(string mealId, string foodName, string foodCalories, string foodGrams, string foodProteins, string foodCarbs, string foodFats)
        {
            //maybe a food controller
            return this.View();
        }

        [HttpPost]
        public IActionResult CreateMeal(string mealName, string mealPlanId)
        {
            //maybe a food controller
            return RedirectToAction("All");
        }
    }
}
