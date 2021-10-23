using System;

namespace SCSS.Application.Admin.Models.DealerInformationModels
{
    public class DealerInformationViewModel
    {
        public Guid Id { get; set; }

        public string DealerName { get; set; }

        public string DealerPhone { get; set; }

        public string ManagedBy { get; set; }

        public int DealerType { get; set; }

        public bool Status { get; set; }

        public string CreatedTime { get; set; }
    }
}
