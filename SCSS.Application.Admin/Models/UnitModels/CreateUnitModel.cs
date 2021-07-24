using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.UnitModels
{
    public class CreateUnitModel
    {
        [TextUtil(50, false)]
        public string Name { get; set; }
    }
}
