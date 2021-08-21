using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public enum FileS3Path
    {
        AdminCategoryImages,
        AdminAccountImages,
        SellerAccountImages,
        DealerAccountImages,
        CollectorAccountImages,
        ImageSliderImages
    }

    public enum RoleEnum
    {
        Admin,
        Collector,
        Dealer,
        Seller
    }

    public enum PrefixFileName
    {
        AdminCategory,
        AdminAccount,
        SellerAccount,
        DealerAccount,
        CollectorAccount,
        ImageSlider
    }
}
