using System;
using System.Collections.Generic;
using System.Text;

namespace Fitnezz.Web.Web.ViewModels.Workouts
{
    public class AllWourkoutsViewModel
    {
        public bool IsDeleted { get; set; }

        public string UserId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int ExercisesCount { get; set; }
    }
}
