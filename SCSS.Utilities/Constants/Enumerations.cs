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
        ImageSliderImages,
        ScrapCollectingRequestImages,
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
        ScrapCategory,
        ScrapCollectingRequest
    }

    public enum CacheRedisKey
    {
        ImageSlider
    }

    public enum Vehicle
    {
        car,
        bike,
        taxi,
        truck,
        hd // for ride hailing vehicles
    }
}
