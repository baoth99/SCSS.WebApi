using Microsoft.AspNetCore.Http;
using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.AdminCategoryModels
{
    public class CreateCategoryAdminModel
    {
        [TextUtil(100, false)]
        public string Name { get; set; }

        public Guid Unit { get; set; }

        [ImageFile]
        public IFormFile Image { get; set; }

        [TextUtil(500, true)]
        public string Description { get; set; }

    }
}
