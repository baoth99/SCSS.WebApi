namespace SCSS.Utilities.Constants
{
    public enum FileS3Path
    {
        ScrapCategoryImages,
        AdminAccountImages,
        SellerAccountImages,
        DealerAccountImages,
        DealerInformationImages,
        CollectorAccountImages,
        ImageSliderImages
    }

    public enum RoleEnum
    {
        Admin,
        Collector,
        Dealer,
        DealerMember,
        Seller
    }

    public enum PrefixFileName
    {
        AdminAccount,
        SellerAccount,
        DealerAccount,
        DealerInformation,
        CollectorAccount,
        ImageSlider,
        ScrapCategory
    }

    public enum CacheRedisKey
    {
        ImageSlider
    }
}
