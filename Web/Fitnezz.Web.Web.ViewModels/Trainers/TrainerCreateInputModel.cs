using System.ComponentModel.DataAnnotations;

namespace Fitnezz.Web.Web.ViewModels.Trainers
{
    public class TrainerCreateInputModel
    {
        [Required]
        [MaxLength(30,ErrorMessage = "Max characters for the name are 30")]
        [MinLength(6, ErrorMessage = "Min characters for the name are 6")]
        public string Name { get; set; }

        [Required]
        [Range(18,55)]
        public int Age { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Min characters for the specialty are 6")]
        public string Specialty { get; set; }

        [Required]
        [Url]
        public string Img { get; set; }

    }
}