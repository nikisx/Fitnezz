using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class TraineesMealPlans : BaseDeletableModel<int>
    {
        public string TraineeId { get; set; }

        public ApplicationUser Trainee { get; set; }

        public int MealPlanId { get; set; }

        public MealPlan MealPlan { get; set; }

    }
}