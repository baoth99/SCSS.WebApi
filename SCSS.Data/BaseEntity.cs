using System;

namespace SCSS.Data
{
    public class BaseEntity
    {
        public DateTime? CreatedTime { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
