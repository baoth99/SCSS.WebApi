using SCSS.Utilities.Constants;
using System.Globalization;

namespace SCSS.Utilities.Extensions
{
    public static class NumberExtension
    {
        public static string ToMoney(this long? val, string culture = Globalization.VN_CULTURE)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo(culture);

            if (!val.HasValue)
            {
                return string.Format("{0}", CommonConstants.Null);
            }
            string result = val.Value.ToString("#,###", cul.NumberFormat);

            return string.Format("{0} vnđ", result);
        }

        public static float KilometerToMeter(this float val)
        {
            return val == NumberConstant.Zero ? NumberConstant.Zero : val * NumberConstant.OneThousand;
        }

        public static float MeterToKilometer(this float val)
        {
            return val == NumberConstant.Zero ? NumberConstant.Zero : (val / NumberConstant.OneThousand);
        }

        public static int ToIntValue(this int? val)
        {
            return val.HasValue ? val.Value : NumberConstant.Zero;
        }
    }
}
