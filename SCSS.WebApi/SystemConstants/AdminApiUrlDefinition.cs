
namespace SCSS.WebApi.SystemConstants
{

    public class AdminCategoryApiUrlDefinition
    {
        public const string AdminCategory = "admin-category";
        public const string Search = AdminCategory + "/search";
        public const string Detail = AdminCategory + "/detail";
        public const string Create = AdminCategory + "/create";
        public const string Edit = AdminCategory + "/edit";
        public const string Remove = AdminCategory + "/remove";
    }

    public class AccountApiUrlDefinition
    {
        public const string Account = "account";
        public const string ChangeStatus = Account + "/change-status";
    }
    public class UnitApiUrlDefinition
    {
        public const string Unit = "unit";
        public const string Search = Unit + "/search";
        public const string Create = Unit + "/create";
        public const string Edit = Unit + "/edit";
        public const string Remove = Unit + "/remove";
    }

    public class AdminDataApiUrlDefinition
    {
        public const string Data = "data";
        public const string Unit = Data + "/units";
        public const string Role = Data + "/roles";
        public const string Image = Data + "/image";
    }
}
;