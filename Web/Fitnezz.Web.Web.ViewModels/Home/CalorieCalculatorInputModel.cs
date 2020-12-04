using System.ComponentModel.DataAnnotations;

namespace Fitnezz.Web.Web.ViewModels.Home
{
    public class CalorieCalculatorInputModel
    {
        [Required]
        public Goals Goal { get; set; }

        public bool WeightAvailable { get; set; }

        [Required]
        public double Weight { get; set; }
    }
}