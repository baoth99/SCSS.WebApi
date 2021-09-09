namespace SCSS.WebApi.SystemConstants
{
    public class ScrapSellerApiUrlDefinition
    {
        private const string Account = "seller/account";
        private const string ImageSlider = "image-slider";
        public static class AccountApiUrl
        {
            public const string RegisterSellerAccount = Account + "/register";
            public const string UpdateSellerAccount = Account + "/update";
        }

        public static class ImageSliderApiUrl
        {
            public const string GetImageSlider = ImageSlider + "/images";
        }

    }

}
