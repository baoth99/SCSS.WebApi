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
    
    public class CommonConstants
    {
        public const string ContentType = "Content-Type";
    }

    public class IdentityServer4Constant
    {
        public const string ClientId = "client_id";
    }

    public class AccountStatus
    {
        public const int NOT_APPROVED = 0;
        public const int APPROVED = 1;
        public const int ACTIVE = 2;
        public const int BANNING = 3;
        public const int DELECTED = 4;
    }

    public class BookingStatus
    {
        public const int PENDING = 0;
        public const int CANCEL_BY_SELLER = 1;
        public const int CANCEL_BY_COLLECTOR = 2;
        public const int APPROVED = 3;
        public const int COMPLETED = 4;
    }

    public class AccountCategoryStatus
    {
        public const int DISABLED = 0;
        public const int ENABLED = 1;
    }

}
