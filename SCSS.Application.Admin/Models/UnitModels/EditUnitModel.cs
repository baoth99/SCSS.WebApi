using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;

namespace SCSS.Application.Admin.Models.UnitModels
{
    public class EditUnitModel
    {
        public Guid Id { get; set; }

        [TextUtil(50, false)]
        public string Name { get; set; }
    }
}
