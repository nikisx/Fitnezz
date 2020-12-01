using System.ComponentModel.DataAnnotations;

namespace Fitnezz.Web.Web.ViewModels
{
    public class CreateCardInputModel
    {
        [Required]
        [Range(18,70)]
        public int Age { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public double Height { get; set; }

        [Required]
        [MinLength(6,ErrorMessage = "The goal should be minimum 6 characters")]
        [MaxLength(30, ErrorMessage = "The goal should be maximum 30 characters")]
        public string Goal { get; set; }

    }
}