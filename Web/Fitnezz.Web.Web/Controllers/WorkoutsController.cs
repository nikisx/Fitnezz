using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class WorkoutsController : Controller
    {
        public IActionResult All()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(string workoutName)
        {
            if (workoutName == null)
            {
                return this.Redirect("/");
            }

            return this.RedirectToAction("All");
        }

        
        [HttpPost]
        public IActionResult AddWorkoutToUser(string username, int workoutId)
        {
         

            return this.RedirectToAction("All");
        }
    }
}
