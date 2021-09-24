
namespace SCSS.Application.Admin.Models.CollectingRequestModels
{
    public class CollectingRequestSearchModel : BaseFilterModel
    {
        public string CollectingRequestCode { get; set; }

        public string RequestedBy { get; set; }

        public string ReceivedBy { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int Status { get; set; }
    }
}
