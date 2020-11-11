using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitnezz.Web.Web.ViewModels.Workouts
{
  
   public class AddExerciseToWorkoutInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Range(1,12)]
        [Required]
        public int Sets { get; set; }

        [Range(0, 30)]
        public int? Reps { get; set; }

        [Range(0, 300)]
        public int? Distance { get; set; }

        [Range(0,30.0)]
        public decimal? Time { get; set; }

        [Required]
        [Url]
        public string Link { get; set; }

        public int WorkoutId { get; set; }
    }
}
