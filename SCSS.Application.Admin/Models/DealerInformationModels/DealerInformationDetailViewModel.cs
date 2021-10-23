using System;

namespace SCSS.Application.Admin.Models.DealerInformationModels
{
    public class DealerInformationDetailViewModel
    {
        public Guid AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountPhone { get; set; }

        public int AccountRole { get; set; }

        // Dealer Information
        public Guid DealerId { get; set; }

        public string DealerName { get; set; }

        public string DealerPhone { get; set; }

        public string DealerAddress { get; set; }

        public decimal? DealerLatitude { get; set; }

        public decimal? DealerLongtitude { get; set; }

        public string DealerImageUrl { get; set; }

        public string DealerCreatedTime { get; set; }

        public int DealerType { get; set; }

        public LeaderInformationModel Leader { get; set; }

        public string DealerOpenTime { get; set; }

        public string DealerCloseTime { get; set; }

        public bool DealerIsActive { get; set; }

    }

    public class LeaderInformationModel
    {
        public Guid DealerId { get; set; }

        public Guid AccountId { get; set; }
    }
}
