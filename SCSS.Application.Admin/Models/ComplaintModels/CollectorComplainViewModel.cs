using System;
using System.Text.Json.Serialization;

namespace SCSS.Application.Admin.Models.ComplaintModels
{
    public class CollectorComplainViewModel
    {
        public Guid? Id { get; set; }

        public string Code { get; set; }

        public string ComplaintContent { get; set; }

        public string CollectorInfo { get; set; }

        public string ComplaintedAccountInfo { get; set; }

        public string RepliedContent { get; set; }

        public bool WasReplied { get; set; }

        public string ComplaintTime { get; set; }

        [JsonIgnore]
        public DateTime? CreatedTime { get; set; }
    }
}
