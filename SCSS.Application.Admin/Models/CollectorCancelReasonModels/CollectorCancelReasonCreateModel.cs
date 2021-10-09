using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.Admin.Models.CollectorCancelReasonModels
{
    public class CollectorCancelReasonCreateModel
    {
        [TextUtil(max: NumberConstant.FiveHundred, isBlank: BooleanConstants.FALSE)]
        public string Content { get; set; }
    }
}
