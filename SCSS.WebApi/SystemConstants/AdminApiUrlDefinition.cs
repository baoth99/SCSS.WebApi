
namespace SCSS.WebApi.SystemConstants
{
    public class AdminApiUrlDefinition
    {
        private const string Hub = "/hubs/admin";
        private const string AdminCategory = "admin-category";
        private const string Account = "account/admin";
        private const string Unit = "unit";
        private const string Data = "data";
        private const string ImageSlider = "image-slider";
        private const string RequestRegister = "request-register";

        public static class HubApiUrl
        {
            public const string AmountNewAccount = Hub + "/amount/new-user";
            public const string AmountBooking = Hub + "/amount/booking";
            public const string AmountTransaction = Hub + "/amount/transaction";
        }

        public static class DashboardApiUrl
        {
            public const string Dashboard = "dashboard";
        }

        public static class ImageSliderApiUrl
        {
            public const string Create = ImageSlider + "/create";
            public const string GetList = ImageSlider + "/list";
            public const string GetImages = ImageSlider + "/images";
            public const string GetImageDetail = ImageSlider + "/detail";
            public const string ChangeItem = ImageSlider + "/change";

        }

        public static class CategoryAdminApiUrl
        {
            public const string Search = AdminCategory + "/search";
            public const string Detail = AdminCategory + "/detail";
            public const string Create = AdminCategory + "/create";
            public const string Edit = AdminCategory + "/edit";
            public const string Remove = AdminCategory + "/remove";
        }

        public static class AccountApiUrl
        {
            public const string ChangeStatus = Account + "/change-status";
            public const string Search = Account + "/search";
            public const string Detail = Account + "/detail";
        }

        public static class RequestRegisterApiUrl
        {
            public const string SearchDealers = RequestRegister + "/dealers";
            public const string SearchCollectors = RequestRegister + "/collectors";
            public const string DealerDetail = RequestRegister + "/dealer-detail";
            public const string CollectorDetail = RequestRegister + "/collector-detail";
        }

        public static class UnitApiUrl
        {
            public const string Search = Unit + "/search";
            public const string Create = Unit + "/create";
            public const string Edit = Unit + "/edit";
            public const string Remove = Unit + "/remove";
        }

        public static class AdminDataApiUrl
        {
            public const string Unit = Data + "/units";
            public const string Role = Data + "/roles";
            public const string Image = Data + "/image";
        }
    }
}
