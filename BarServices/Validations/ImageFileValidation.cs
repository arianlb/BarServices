using System.ComponentModel.DataAnnotations;

namespace BarServices.Validations
{
    public class ImageFileValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) { return ValidationResult.Success; }

            IFormFile formFile = value as IFormFile;
            if (formFile is null) { return ValidationResult.Success; }

            string[] imageTypes = new string[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!imageTypes.Contains(formFile.ContentType))
            {
                return new ValidationResult("The type of image is not correct");
            }

            return ValidationResult.Success;
        }
    }
}
