
namespace SCSS.Application.Admin.Models.ScrapCategoryModels
{
    public class ScrapCategorySearchModel : BaseFilterModel
    {
        public string Name { get; set; }

        public int Status { get; set; }

        public int Role { get; set; }

        public string PhoneCreatedBy { get; set; }

        public string CreatedBy { get; set; }

    }
}
