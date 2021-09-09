using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace SCSS.Validations.ValidationAttributes
{
    public class ScrapCategoryDetailAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var scrapCategoryDetail = ((IEnumerable)value).Cast<object>().ToList();

            
            if (scrapCategoryDetail.Count == 0)
            {
                return new ValidationResult(InvalidScrapCategory.Empty);
            }
            
            var errorList = scrapCategoryDetail.Where(x => (long)x.GetPropertyValue("Price") < 0);
            if (errorList.Any())
            {
                return new ValidationResult(InvalidScrapCategory.Empty);
            }
           
            return ValidationResult.Success;
        }
    }
}
