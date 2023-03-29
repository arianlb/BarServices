using BarServices.Validations;

namespace BarServices.DTOs
{
    public class ProductPictureDTO
    {
        [FileSizeValidation(15)]
        [ImageFileValidation]
        public IFormFile Picture { get; set; }
    }
}
