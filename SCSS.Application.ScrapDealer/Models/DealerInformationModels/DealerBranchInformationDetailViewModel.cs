using System;

namespace SCSS.Application.ScrapDealer.Models.DealerInformationModels
{
    public class DealerBranchInformationDetailViewModel
    {
        public Guid Id { get; set; }

        public string DealerBranchImageUrl { get; set; }

        public string DealerBranchName { get; set; }

        public string DealerBranchPhone { get; set; }

        public string DealerBranchAddress { get; set; }

        public string DealerBranchOpenTime { get; set; }

        public string DealerBranchCloseTime { get; set; }

        public DealerAccountBranchInformationViewModel DealerAccountBranch { get; set;}
    }

    public class DealerAccountBranchInformationViewModel
    {
        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
