using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Workouts()
        {
            return this.View();
        }

        public IActionResult Workout(int id)
        {
            this.ViewBag.Workout = id;
            return this.View();
        }

        public IActionResult MealPlans()
        {
            return this.View();
        }

        public IActionResult MealPlan(int id)
        {
            this.ViewBag.Id = id;
            return this.View();
        }
    }
}
