
namespace SCSS.WebApi.SystemConstants
{
    public class ScrapCollectorApiUrlDefinition
    {
        private const string ScrapCategory = "scrap-category";
        private const string Account = "collector/account";

        public static class AccountApiUrl
        {
            public const string RegisterCollectorAccount = Account + "/register";
            public const string UpdateCollectorAccount = Account + "/update";
        }

        public static class ScrapCategoryUrl
        {
            public const string Create = ScrapCategory + "/create";
            public const string Get = ScrapCategory + "/get";
            public const string GetDetail = ScrapCategory + "/get-detail";
            public const string CheckName = ScrapCategory + "/check-name";
            public const string Update = ScrapCategory + "/update";
            public const string UploadImage = ScrapCategory + "/upload-image";
            public const string GetImage = ScrapCategory + "/get-image";
            public const string Remove = ScrapCategory + "/remove";
        }
    }
}
