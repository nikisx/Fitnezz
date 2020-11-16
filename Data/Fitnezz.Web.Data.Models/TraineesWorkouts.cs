using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class TraineesWorkouts : BaseDeletableModel<int>
    {
        public string TraineeId { get; set; }

        public ApplicationUser Trainee { get; set; }

        public int WorkoutId { get; set; }

        public Workout Workout { get; set; }
    }
}