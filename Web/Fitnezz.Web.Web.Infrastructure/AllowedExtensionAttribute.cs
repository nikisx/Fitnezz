using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Fitnezz.Web.Web.Infrastructure
{
    public class AllowedExtensionAttribute : ValidationAttribute
    {
        private readonly string extension;

        public AllowedExtensionAttribute(string extension)
        {
            this.extension = extension;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                if (this.extension != fileExtension.ToLower())
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Allowed photo extension is jpg";
        }
    }
}