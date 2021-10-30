using System;
namespace SCSS.Utilities.Constants
{
    public class NumberConstant
    {
        public const int Zero = 0;
        public const int One = 1;
        public const int Two = 2;
        public const int Four = 4;
        public const int NegativeOne = -1;
        public const int Five = 5;
        public const int Seven = 7;
        public const int Ten = 10;
        public const int Fifteen = 15;
        public const int Twenty = 20;
        public const int TwentyFour = 24;
        public const int Sixty = 60;
        public const int FiveHundred = 500;
        public const int OneThousand = 1000;
        public const int TenThousand = 10000;
    }

    public class BooleanConstants
    {
        public const bool TRUE = true;
        public const bool FALSE = false;
    }
    
    public class DateTimeVN
    {
        public static DateTime DATETIME_NOW => TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfoId.SE_ASIA));

        public static DateTime DATE_NOW => DATETIME_NOW.Date;

        public static TimeSpan TIMESPAN_NOW => DATETIME_NOW.TimeOfDay;

        public static DateTime DATE_FROM => DATETIME_NOW.Date;

        public static DateTime DATE_TO => DATETIME_NOW.Date.AddHours(24);
        
    }

    public class DateTimeFormat
    {
        public const string Format01 = @"MM-dd-yyyy-hh:mm:tt";
        public const string DD_MM_yyyy_time_tt = @"dd-MM-yyyy hh:mm tt";
        public const string DD_MM_yyyy_time = @"dd-MM-yyyy hh:mm";
        public const string DD_MM_yyyy = @"dd-MM-yyyy";
        public const string yyyy_MM_DD = @"yyyy-MM-dd";
        public const string DD_MMM_yyyy = @"dd MMM, yyyy";
        public const string DDMMyyyyhhmmss = @"ddMMyyyyhhmmss";
        public const string DDD_DD_MM_yyy_HH_mm = @"ddd, dd MM yyyy lúc HH:mm";
        public const string DDD_DD_MMM_yyy_HH_mm = @"ddd, dd MMM yyyy";
        public const string DDD_DD_MM_yyyy = @"ddd, dd/MM/yyyy";
        public const string DDD_dd_MMM = "ddd, dd MMM";
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
    }

    public class DefaultConstant
    {
        public const float TotalPoint = 0;
        public const double Radius = 5;
    }

    public class RegularExpression
    {
        public const string PhoneRegex = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
        public const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
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

    public class CollectingRequestType
    {
        public const int CURRENT_REQUEST = 1;
        public const int MAKE_AN_APPOINTMENT = 2;
        public const int SWITCH_TO_CURRENT_REQUEST = 3; // MAKE_AN_APPOINTMENT => CURRENT_REQUEST 
    }

    public class TransactionType
    {
        public const int SELL_COLLECT = 1;
        public const int COLLECT_DEAL = 2;
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
        public const string CollectingRequestDateDESC = "CollectingRequestDate DESC";
        public const string ApprovedTimeDESC = "ApprovedTime DESC";
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

    public class FeedbackStatus
    {
        public const int HaveNotGivenFeedbackYet = 1;
        public const int HaveGivenFeedback = 2;
        public const int TimeUpToGiveFeedback = 3;
    }

    public class ComplaintStatus
    {
        public const int CanNotGiveComplaint = 1;
        public const int CanGiveComplaint = 2;
        public const int HaveGivenComplaint = 3;
        public const int AdminReplied = 4;
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

    public class TimeZoneInfoId
    {
        public const string SE_ASIA = "SE Asia Standard Time";
        public const string SINGAPORE = "Singapore Standard Time";
    }

    public class ImageFileConstants
    {
        public const string PNG = ".png";
        public const string JPEG = ".jpeg";
        public const string JPG = ".jpg";
    }

    public class ContentTypeString
    {
        public const string JpgImageContentType = "image/jpeg";
        public const string PngImageContentType = "image/png";
        public const string JpegImageContentType = "image/jpeg";
    }

    public class ReminderType
    {
        public const int PreviousFromTime = 0;
        public const int LaterToTime = 1;
    }

    public class SellerAppScreen
    {
        public const string ActivityScreen = "1";
    }

    public class CollectorAppScreen
    {
        public const string CollectingRequestScreen = "1";
        public const string HistoryScreen = "2";
    }

    public class DealerAppScreen
    {

    }
}
