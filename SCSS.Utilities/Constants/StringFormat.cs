using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class DateCodeFormat
    {
        public const string DDMMYYYY = "{0}{1}{2}";
        public const string DDMM = "{0}{1}";
    }

    public class TimeSpanCodeFormat
    {
        public const string HHMMSS = "{0}{1}{2}";
        public const string HHMM = "{0}{1}";
    }

    public class GenerationCodeFormat
    {
        public const string COLLECTING_REQUEST_CODE = "SCR-{0}{1}{2}{3}";
        public const string PROMOTION_CODE = "KM-{0}-{1}{2}{3}";
        public const string SELL_COLLECT_TRASACTION_CODE = "SCT-{0}{1}";
    }

    public class GoongMapRestApiFormat
    {
        public static string DestinationCoordinate => "{0},{1}";
        public static string DistanceMatrixEndpoint(string destinations) => "{0}{1}?origins={2},{3}&destinations=" + destinations + "&vehicle={4}&api_key={5}";
        public static string DirectionEndpoint(string destinations) => "{0}{1}?origin={2},{3}&destination=" + destinations +"&alternatives={4}&vehicle={5}&api_key={6}";
    }
}
