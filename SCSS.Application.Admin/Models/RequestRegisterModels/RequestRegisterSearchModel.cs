
namespace SCSS.Application.Admin.Models.RequestRegisterModels
{
    public class CollectorRequestRegisterSearchModel : BaseFilterModel
    {
        public string Phone { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

    }

    public class DealerRequestRegisterSearchModel : BaseFilterModel
    {
        public string Phone { get; set; }

        public string Name { get; set; }

        public string DealerName { get; set; }

        public int Status { get; set; }

    }
}
