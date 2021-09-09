using System;
namespace SCSS.Utilities.Constants
{
    public class BooleanConstants
    {
        public const bool TRUE = true;
        public const bool FALSE = false;
    }
    
    public class DateTimeInDay
    {
        public static DateTime DATEFROM = DateTime.Now.Date;
        public static DateTime DATETO = DateTime.Now.Date.AddHours(24);
    }

    public class DateTimeFormat
    {
        public const string Format01 = "MM-dd-yyyy-hh:mm:tt";
        public const string DD_MM_yyyy_time = "dd/MM/yyyy hh:mm tt";
        public const string DD_MM_yyyy = "dd/MM/yyyy";
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

    public class Gender
    {
        public const int MALE = 1;
        public const int FEMALE = 2;

        public const string MALE_TEXT = "Male";
        public const string FEMALE_TEXT = "Female";
    }

    public class BookingStatus
    {
        public const int PENDING = 1;
        public const int CANCEL_BY_SELLER = 2;
        public const int CANCEL_BY_COLLECTOR = 3;
        public const int APPROVED = 4;
        public const int COMPLETED = 5;
    }

    public class AccountCategoryStatus
    {
        public const int DISABLED = 1;
        public const int ENABLED = 2;
    }

    public class AccountRole
    {
        public const int ADMIN = 1;
        public const int SELLER = 2;
        public const int DEALER = 3;
        public const int COLLECTOR = 4;
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
}
