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
        public DateTime? CreateTime { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifyTime { get; set; }

        public Guid? ModifyBy { get; set; }

    }
}
