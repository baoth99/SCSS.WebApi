using GeoCoordinatePortable;
using SCSS.Utilities.Constants;

namespace SCSS.Utilities.Helper
{
    public class CoordinateHelper
    {
        public static double DistanceTo(decimal? geoLat, decimal? geoLong, decimal? desLat, decimal? desLong)
        {
            if (!geoLat.HasValue || !geoLong.HasValue || !desLat.HasValue || !desLong.HasValue)
            {
                return NumberConstant.NegativeOne;
            }

            var geoCoordinate = new GeoCoordinate((double)geoLat, (double)geoLong);
            var desCoordinate = new GeoCoordinate((double)desLat, (double)desLong);

            return geoCoordinate.GetDistanceTo(desCoordinate);

        }

        public static bool IsInRadius(decimal? geoLat, decimal? geoLong, decimal? desLat, decimal? desLong, double radius)
        {
            var distance = DistanceTo(geoLat, geoLong, desLat, desLong);

            if (distance < NumberConstant.Zero)
            {
                return BooleanConstants.FALSE;
            }

            return distance <= radius;

        }
    }
}
