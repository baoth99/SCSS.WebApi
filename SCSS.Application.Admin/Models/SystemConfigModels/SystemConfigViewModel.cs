using System;
using System.Collections.Generic;

namespace SCSS.Application.Admin.Models.SystemConfigModels
{
    public class SystemConfigViewModel
    {
        public Guid Id { get; set; }

        public int RequestQuantity { get; set; }

        public int ReceiveQuantity { get; set; }

        public int MaxNumberOfRequestDays { get; set; }

        public string OperatingTimeFrom { get; set; }

        public string OperatingTimeTo { get; set; }

        public string ActiveTime { get; set; }

        public List<SystemConfigHistoryViewModel> Histories { get; set; }

    }

    public class SystemConfigHistoryViewModel
    {
        public int RequestQuantity { get; set; }

        public int ReceiveQuantity { get; set; }

        public int MaxNumberOfRequestDays { get; set; }

        public string OperatingTimeFrom { get; set; }

        public string OperatingTimeTo { get; set; }

        public string DeActiveTime { get; set; }

        public string DeActiveBy { get; set; }
    }
}
