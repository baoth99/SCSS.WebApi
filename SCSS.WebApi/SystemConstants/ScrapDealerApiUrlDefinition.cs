namespace SCSS.WebApi.SystemConstants
{
    public class ScrapDealerApiUrlDefinition
    {
        private const string Account = "dealer/account";
        private const string ScrapCategory = "scrap-category";

        public static class AccountApiUrl
        {
            public const string RegisterDealerAccount = Account + "/register";
            public const string UpdateDealerAccount = Account + "/update";
        }


        public static class ScrapCategoryUrl
        {
            public const string Create = ScrapCategory + "/create";
            public const string Get = ScrapCategory + "/get";
            public const string GetDetail = ScrapCategory + "/get-detail";
            public const string Update = ScrapCategory + "/update";
            public const string UploadImage = ScrapCategory + "/upload-image";
            public const string GetImage = ScrapCategory + "/get-image";
            public const string Remove = ScrapCategory + "/remove";
        }

    }
}
