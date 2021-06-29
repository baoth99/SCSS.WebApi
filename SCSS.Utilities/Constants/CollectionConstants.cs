using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class CollectionConstants
    {
        public static readonly List<int> AccountStatusCollection = new List<int>()
        {
            AccountStatus.NOT_APPROVED,
            AccountStatus.ACTIVE,
            AccountStatus.BANNING,
        };

        public static readonly List<string> FileS3PathCollection = new List<string>()
        {
            FileS3Path.AccountImages.ToString(),
            FileS3Path.AdminCategoryImages.ToString(),
        };


        public static readonly List<string> ImageExtensions = new List<string>()
        {
            ImageFileConstants.JPEG,
            ImageFileConstants.JPG,
            ImageFileConstants.PNG
        };

        public static readonly List<int> BookingStatusCollection = new List<int>()
        {
            BookingStatus.PENDING,
            BookingStatus.CANCEL_BY_SELLER,
            BookingStatus.CANCEL_BY_COLLECTOR,
            BookingStatus.APPROVED,
            BookingStatus.COMPLETED
        };

        public static readonly List<int> AccountCategoryStatusCollection = new List<int>()
        {
            AccountCategoryStatus.DISABLED,
            AccountCategoryStatus.ENABLED
        };
    }

    public class DictionaryConstants
    {
        public static readonly Dictionary<string, int> AccountStatusCollection = new Dictionary<string, int>()
        {
            {AccountRole.ADMIN_TEXT, AccountRole.ADMIN },
            {AccountRole.SELLER_TEXT, AccountRole.SELLER },
            {AccountRole.DEALER_TEXT, AccountRole.DEALER },
            {AccountRole.COLLECTOR_TEXT, AccountRole.COLLECTOR }
        };

    }
}
