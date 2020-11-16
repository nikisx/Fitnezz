using System;
using System.Collections.Generic;
using System.Text;
using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
   public class Workout : BaseDeletableModel<int>
    {
        public Workout()
        {
            this.Exercises = new HashSet<Exercise>();
            this.Trainees = new HashSet<TraineesWorkouts>();
        }

        public string Name { get; set; }

        public ICollection<Exercise> Exercises { get; set; }

        public ICollection<TraineesWorkouts> Trainees { get; set; }
    }
}
