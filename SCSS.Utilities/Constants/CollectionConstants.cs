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

        public static readonly List<int> CollectingRequestHistory = new List<int>()
        {
            CollectingRequestStatus.COMPLETED,
            CollectingRequestStatus.CANCEL_BY_COLLECTOR,
            CollectingRequestStatus.CANCEL_BY_SYSTEM
        };

        public static readonly List<string> FileS3PathCollection = new List<string>()
        {
            FileS3Path.AdminAccountImages.ToString(),
            FileS3Path.CollectorAccountImages.ToString(),
            FileS3Path.DealerAccountImages.ToString(),
            FileS3Path.SellerAccountImages.ToString(),
            FileS3Path.ScrapCategoryImages.ToString(),
            FileS3Path.DealerInformationImages.ToString(),
            FileS3Path.ImageSliderImages.ToString(),
            FileS3Path.ScrapCollectingRequestImages.ToString()
        };


        public static readonly List<string> ImageExtensions = new List<string>()
        {
            ImageFileConstants.JPEG,
            ImageFileConstants.JPG,
            ImageFileConstants.PNG
        };

        public static readonly List<int> CollectingRequestStatusCollection = new List<int>()
        {
            CollectingRequestStatus.PENDING,
            CollectingRequestStatus.CANCEL_BY_SELLER,
            CollectingRequestStatus.CANCEL_BY_COLLECTOR,
            CollectingRequestStatus.CANCEL_BY_SYSTEM,
            CollectingRequestStatus.APPROVED,
            CollectingRequestStatus.COMPLETED
        };

        public static readonly List<int> GenderCollection = new List<int>()
        {
            Gender.FEMALE,
            Gender.MALE
        };

        public static int[] PromotionStatusCollection => new int[2]
        {
            PromotionStatus.ACTIVE,
            PromotionStatus.DEACTIVE
        };
            

        public static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static List<T> Empty<T>() => Enumerable.Empty<T>().ToList();

        public static readonly List<int> CompletedCRActivity = new List<int>() 
        { 
            CollectingRequestStatus.COMPLETED, 
            CollectingRequestStatus.CANCEL_BY_SELLER, 
            CollectingRequestStatus.CANCEL_BY_COLLECTOR,
            CollectingRequestStatus.CANCEL_BY_SYSTEM,
        };


        public static readonly List<int> RemainingCollectingRequest = new List<int>()
        {
            CollectingRequestStatus.PENDING,
            CollectingRequestStatus.APPROVED,
        };


        public static readonly List<CacheRedisKey> TransactionServiceFees = new List<CacheRedisKey>()
        {
            CacheRedisKey.SellCollectTransactionServiceFee,
            CacheRedisKey.CollectDealTransactionServiceFee,
        };

        public static readonly List<CacheRedisKey> TransactionAwardAmounts = new List<CacheRedisKey>()
        {
            CacheRedisKey.SellCollectTransactionAwardAmount,
            CacheRedisKey.CollectDealTransactionAwardAmount,
        };

        public static readonly List<int> CurrentRequests = new List<int>()
        {
            CollectingRequestType.CURRENT_REQUEST,
            CollectingRequestType.SWITCH_TO_CURRENT_REQUEST
        };

        public static readonly List<int> Appointments = new List<int>()
        {
            CollectingRequestType.MAKE_AN_APPOINTMENT
        };
    }

    public class DictionaryConstants
    {      
        public static readonly Dictionary<string, int> AccountStatusCollection = new Dictionary<string, int>()
        {
            {AccountRoleConstants.ADMIN, AccountRole.ADMIN },
            {AccountRoleConstants.SELLER, AccountRole.SELLER },
            {AccountRoleConstants.DEALER, AccountRole.DEALER },
            {AccountRoleConstants.COLLECTOR, AccountRole.COLLECTOR },
            {AccountRoleConstants.DEALER_MEMBER, AccountRole.DEALER_MEMBER }
        };

        public static Dictionary<string, string> FirebaseCustomData(string appScreen, string id) => new Dictionary<string, string>()
        {
            {"screen", appScreen },
            {"id", id }
        };
    }

}
