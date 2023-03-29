using System.ComponentModel.DataAnnotations;

namespace BarServices.Validations
{
    public class FileSizeValidation : ValidationAttribute
    {
        private readonly int size;

        public FileSizeValidation(int Size)
        {
            size = Size;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is null) { return ValidationResult.Success; }

            IFormFile formFile = value as IFormFile;
            if(formFile is null) { return ValidationResult.Success; }

            if(formFile.Length > size * 1024 * 1024)
            {
                return new ValidationResult($"The maximum file size should not be greater than {size}mb");
            }

            return ValidationResult.Success;
        }
    }
}
