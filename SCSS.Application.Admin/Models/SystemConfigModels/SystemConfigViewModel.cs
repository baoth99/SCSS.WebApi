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

        public int CancelTimeRange { get; set; }

        public int TimeRangeRequestNow { get; set; }

        public int FeedbackDeadline { get; set; }

        public float? NearestDistance { get; set; }

        public float? NearestDistanceOfAppointment { get; set; }

        public float? PriorityRating { get; set; }

        public double AvailableRadius { get; set; }

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

        public int CancelTimeRange { get; set; }

        public int TimeRangeRequestNow { get; set; }

        public int FeedbackDeadline { get; set; }

        public float? NearestDistance { get; set; }

        public float? NearestDistanceOfAppointment { get; set; }

        public float? PriorityRating { get; set; }

        public double AvailableRadius { get; set; }

        public string DeActiveTime { get; set; }

        public string DeActiveBy { get; set; }
    }
}
