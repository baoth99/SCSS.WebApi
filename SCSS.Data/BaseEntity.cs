using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
