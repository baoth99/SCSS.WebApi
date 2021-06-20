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
            AccountStatus.APPROVED,
            AccountStatus.ACTIVE,
            AccountStatus.BANNING,
            AccountStatus.DELECTED
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
}
