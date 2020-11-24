using System.ComponentModel.DataAnnotations;
using Fitnezz.Web.Web.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Fitnezz.Web.Web.ViewModels.Classes
{
    public class ClassCreateInputModel
    {
        [MinLength(3,ErrorMessage = "Name should me min 3 characters")]
        [MaxLength(10, ErrorMessage = "Name should me max 10 characters")]
        [Required]
        public string Name { get; set; }

        [Required]
        public Days DayOfWeek { get; set; }

        [Required]
        [Range(9, 20)]
        public int StartHour { get; set; }

        [Required]
        [Range(11, 21)]
        public int EndHour { get; set; }

        [Required]
        [AllowedExtension(".jpg")]
        public IFormFile Image { get; set; }
    }
}