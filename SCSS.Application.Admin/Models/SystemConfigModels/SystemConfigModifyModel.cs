using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.Admin.Models.SystemConfigModels
{
    public class SystemConfigModifyModel
    {
        public int RequestQuantity { get; set; }

        public int ReceiveQuantity { get; set; }

        public int CancelRangeTime { get; set; }

        public int MaxNumberOfRequestDays { get; set; }

        [TimeSpanValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string OperatingTimeFrom{ get; set; }

        [TimeSpanValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string OperatingTimeTo{ get; set; }

    }
}
