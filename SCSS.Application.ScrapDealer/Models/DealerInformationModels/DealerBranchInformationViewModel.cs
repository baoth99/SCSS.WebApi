using System;


namespace SCSS.Application.ScrapDealer.Models.DealerInformationModels
{
    public class DealerBranchInformationViewModel
    {
        public Guid Id { get; set; }

        public string DealerBranchName { get; set; }

        public string DealerBranchImageUrl { get; set; }

        public string DealerBranchAddress { get; set; }
    }
}
