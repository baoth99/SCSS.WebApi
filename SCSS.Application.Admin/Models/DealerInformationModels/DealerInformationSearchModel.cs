namespace SCSS.Application.Admin.Models.DealerInformationModels
{
    public class DealerInformationSearchModel : BaseFilterModel
    {
        public string DealerName { get; set; }

        public string ManagedBy { get; set; }

        public string DealerPhone { get; set; }

        public string DealerAddress { get; set; }

        public int DealerType { get; set; }

        public bool? Status { get; set; }

    }
}
