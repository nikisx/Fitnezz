using System;
using System.Collections.Generic;
using System.Text;
using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class Exercise : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public int? Reps { get; set; }

        public int? Sets { get; set; }

        public int? Distance { get; set; }

        public decimal? Time { get; set; }

        public string Link { get; set; }

        public int WorkoutId { get; set; }

        public virtual Workout Workout { get; set; }
    }
}
