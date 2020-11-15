using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class TrainersController : Controller
    {
        public IActionResult All()
        {
            return View();
        }

        public IActionResult Hire(string id)
        {
            return RedirectToAction("All");
        }

        [HttpPost]
        public IActionResult AddWorkoutToUser(string username, int workoutId)
        {

            return this.RedirectToAction("All");
        }
    }
}
