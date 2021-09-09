using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes;
using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;
using System.Collections.Generic;

namespace SCSS.Application.ScrapCollector.Models.ScrapCategoryModels
{
    public class ScrapCategoryUpdateModel
    {
        public Guid Id { get; set; }

        [TextUtil(100, BooleanConstants.FALSE)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        [ScrapCategoryDetail]
        public List<ScrapCategoryDetailUpdateModel> Details { get; set; }
    }

    public class ScrapCategoryDetailUpdateModel 
    {
        public Guid Id { get; set; }

        public string Unit { get; set; }

        public long? Price { get; set; }

        public int? Status { get; set; }
    }
}
