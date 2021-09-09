using Microsoft.AspNetCore.Http;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.Admin.Models
{
    public class ImageUploadModel
    {
        [ImageFile]
        public IFormFile Image { get; set; }
    }
}
