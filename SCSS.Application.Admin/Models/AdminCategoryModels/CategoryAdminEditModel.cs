using Microsoft.AspNetCore.Http;
using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.AdminCategoryModels
{
    public class CategoryAdminEditModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        [ImageFile]
        public IFormFile ImageFile { get; set; }
    }
}
