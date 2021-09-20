using System;
namespace SCSS.Utilities.Constants
{

    public class NumberConstant
    {
        public const int Zero = 0;
        public const int Ten = 10;
        public const int OneThousand = 1000;
    }

    public class BooleanConstants
    {
        public const bool TRUE = true;
        public const bool FALSE = false;
    }
    
    public class DateTimeInDay
    {
        public static DateTime DATEFROM = DateTime.Now.Date;
        public static DateTime DATETO = DateTime.Now.Date.AddHours(24);
        public static DateTime DATE_NOW = DateTime.Now.Date;
        public static DateTime DATETIME_NOW = DateTime.Now;
        public static TimeSpan TIMESPAN_NOW = DateTime.Now.TimeOfDay;
    }

    public class DateTimeFormat
    {
        public const string Format01 = @"MM-dd-yyyy-hh:mm:tt";
        public const string DD_MM_yyyy_time = @"dd/MM/yyyy hh:mm tt";
        public const string DD_MM_yyyy = @"dd/MM/yyyy";
    }

    public class TimeSpanFormat
    {
        public const string HH_MM_TT = @"hh\:mm";
        public const string HH_MM = @"hh\:mm";
        public const string HH_MM_SS = @"hh\:mm\:ss";
    }

    public class CommonConstants
    {
        public const int Zero = 0;
        public const string ContentType = "Content-Type";
        public const string Null = "N/A";
        public const string GoogleCredentials = "GOOGLE_APPLICATION_CREDENTIALS";
    }

    public class DefaultConstant
    {
        public const float TotalPoint = 0;
    }

    public class RegularExpression
    {
        public const string PhoneRegex = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
        public const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    }

    public class ImageFileConstants
    {
        public const string PNG = ".png";
        public const string JPEG = ".jpeg";
        public const string JPG = ".jpg";
    }

    public class ScrapCategoryStatus
    {
        public const int ALL = 0;
        public const int ACTIVE = 1;
        public const int DEACTIVE = 2;
    }

    public class AccountStatus
    {
        public const int NOT_APPROVED = 1;
        public const int ACTIVE = 2;
        public const int BANNING = 3;
        public const int REJECT = 4;
    }

    public class PromotionStatus
    {
        public const int ACTIVE = 1;
        public const int DEACTIVE = 2;
    }

    public class Gender
    {
        public const int MALE = 1;
        public const int FEMALE = 2;

        public const string MALE_TEXT = "Male";
        public const string FEMALE_TEXT = "Female";
    }

    public class CollectingRequestStatus
    {
        public const int PENDING = 1;
        public const int CANCEL_BY_SELLER = 2;
        public const int CANCEL_BY_COLLECTOR = 3;
        public const int APPROVED = 4;
        public const int COMPLETED = 5;
        public const int CANCEL_BY_SYSTEM = 6;
    }

    public class DealerType
    {
        public const int LEADER = 1;
        public const int MEMBER = 2;
    }

    public class AccountRole
    {
        public const int ADMIN = 1;
        public const int SELLER = 2;
        public const int DEALER = 3;
        public const int COLLECTOR = 4;
        public const int DEALER_MEMBER = 5;
    }

    public class ClientIdConstant
    {
        public const string SellerMobileApp = "SCSS-Seller-Mobile";
        public const string CollectorMobileApp = "SCSS-Collector-Mobile";
        public const string DealerMobileApp = "SCSS-Dealer-Mobile";
        public const string WebAdmin = "SCSS-WebAdmin-FrontEnd";
    }

    public class DefaultSort
    {
        public const string CreatedTimeDESC = "CreatedTime DESC";
    }

    public class MarkConstant
    {
        public const string NO_WHITE_SPACE = "";
        public const string WHITE_SPACE = " ";
        public const string COMMA = ",";
        public const string DOT = ".";
        public const string COLON = ":";
        public const string SEMICOLON = ":";
        public const string EXCLAMATION_MARK = "!";
        public const string HYPHEN = "-";
        public const string PERCENT = "%";
        public const string SLASH = "|";
    }

    public class RequestScrapCollecting
    {
        public const int SevenDays = 7;
        public const double FifteenMinutes = 15;
    }

    public class Globalization
    {
        public const string VN_CULTURE = "vi-VN";
    }

}
