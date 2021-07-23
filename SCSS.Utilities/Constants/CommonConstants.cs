using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class BooleanConstants
    {
        public const bool TRUE = true;
        public const bool FALSE = false;
    }
    
    public class DateTimeFormat
    {
        public const string Format01 = "MM-dd-yyyy-hh:mm:tt";
        public const string DD_MM_yyyy_time = "dd/MM/yyyy hh:mm tt";
    }

    public class CommonConstants
    {
        public const string ContentType = "Content-Type";
        public const string Null = "N/A";
    }

    public class ImageFileConstants
    {
        public const string PNG = ".png";
        public const string JPEG = ".jpeg";
        public const string JPG = ".jpg";
    }

    public class AccountStatus
    {
        public const int NOT_APPROVED = 1;
        public const int ACTIVE = 2;
        public const int BANNING = 3;
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
}
