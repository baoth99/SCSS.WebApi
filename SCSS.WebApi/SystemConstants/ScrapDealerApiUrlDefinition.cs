namespace SCSS.WebApi.SystemConstants
{
    public class ScrapDealerApiUrlDefinition
    {
        private const string Account = "dealer/account";
        private const string ScrapCategory = "scrap-category";
        private const string DealerInformation = "dealer-information";


        public static class DataApiUrl
        {
            public const string GetImage = "image/get";
        }

        public static class AccountApiUrl
        {
            public const string RegisterDealerAccount = Account + "/register";
            public const string UpdateDealerAccount = Account + "/update";
            public const string UploadImage = Account + "/upload-image";
        }

        public static class DealerInformationApiUrl
        {
            public const string UploadImage = DealerInformation + "/upload-image";
        }

        public static class ScrapCategoryUrl
        {
            public const string Create = ScrapCategory + "/create";
            public const string CheckName = ScrapCategory + "/check-name";
            public const string Get = ScrapCategory + "/get";
            public const string GetDetail = ScrapCategory + "/get-detail";
            public const string Update = ScrapCategory + "/update";
            public const string UploadImage = ScrapCategory + "/upload-image";
            public const string GetImage = ScrapCategory + "/get-image";
            public const string Remove = ScrapCategory + "/remove";
        }

    }
}
