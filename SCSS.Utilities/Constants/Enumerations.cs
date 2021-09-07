namespace SCSS.Utilities.Constants
{
    public enum FileS3Path
    {
        ScrapCategoryImages,
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
        AdminAccount,
        SellerAccount,
        DealerAccount,
        CollectorAccount,
        ImageSlider
    }

    public enum CacheRedisKey
    {
        ImageSlider
    }
}
