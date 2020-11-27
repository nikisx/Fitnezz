using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fitnezz.Web.Web.ViewModels
{
    public class ProfileUpdateInputModel
    {
        [Required]
        public int Age { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public double Height { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Goal { get; set; }

        [Required]
        [DisplayName("Username")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }
    }
}