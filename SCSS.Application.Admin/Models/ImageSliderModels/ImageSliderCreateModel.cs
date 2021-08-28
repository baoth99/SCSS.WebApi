using Microsoft.AspNetCore.Http;
using SCSS.Validations.ValidationAttributes.CommonValidations;


namespace SCSS.Application.Admin.Models.ImageSliderModels
{
    public class ImageSliderCreateModel
    {
        [ImageFile]
        public IFormFile Image { get; set; }

    }
}
