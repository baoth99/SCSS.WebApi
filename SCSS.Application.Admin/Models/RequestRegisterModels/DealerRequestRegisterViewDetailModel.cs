using System;

namespace SCSS.Application.Admin.Models.RequestRegisterModels
{
    public class DealerRequestRegisterViewDetailModel
    {
        public Guid Id { get; set; }

        public string AccountName { get; set; }

        public string AccountPhone { get; set; }

        public string AccountAddress { get; set; }

        public string BirthDate { get; set; }

        public string IDCard { get; set; }

        public int? Gender { get; set; }

        public int? AccountStatus { get; set; }
   
        public DealerInformationRequestViewModel DealerInformation { get; set; }

        public int? DealerType { get; set; }

        public DealerLeaderViewDetailModel DealerLeader { get; set; }
    }

    public class DealerLeaderViewDetailModel
    {
        public Guid DealerId { get; set; }

        public Guid ManagerId { get; set; }

        public string DealerName { get; set; }

        public string ManagerName { get; set; }

        public string ManagerPhone { get; set; }
    }

    public class DealerInformationRequestViewModel
    {
        public string DealerName { get; set; }

        public string DealerPhone { get; set; }

        public string DealerImageUrl { get; set; }

        public string DealerAddress { get; set; }

        public decimal? DealerLatitude { get; set; }

        public decimal? DealerLongitude { get; set; }
    }
}
