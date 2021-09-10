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
            FileS3Path.AdminAccountImages.ToString(),
            FileS3Path.CollectorAccountImages.ToString(),
            FileS3Path.DealerAccountImages.ToString(),
            FileS3Path.SellerAccountImages.ToString(),
            FileS3Path.ScrapCategoryImages.ToString(),
            FileS3Path.ImageSliderImages.ToString()
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

        public static readonly List<int> GenderCollection = new List<int>()
        {
            Gender.FEMALE,
            Gender.MALE
        };

        public static List<T> Empty<T>() => Enumerable.Empty<T>().ToList();
    }

    public class DictionaryConstants
    {      

        public static readonly Dictionary<string, int> AccountStatusCollection = new Dictionary<string, int>()
        {
            {AccountRoleConstants.ADMIN, AccountRole.ADMIN },
            {AccountRoleConstants.SELLER, AccountRole.SELLER },
            {AccountRoleConstants.DEALER, AccountRole.DEALER },
            {AccountRoleConstants.COLLECTOR, AccountRole.COLLECTOR }
        };

    }
}
