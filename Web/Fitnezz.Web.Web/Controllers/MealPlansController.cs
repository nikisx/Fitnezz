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

        public IActionResult AddMealPlanToUser(string username, int mealPlanId)
        {
            return this.RedirectToAction("All");
        }
    }
}
