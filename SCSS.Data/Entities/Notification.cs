using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("Notification")]
    public class Notification : BaseEntity
    {
        public Guid Id { get; set; }
    }
}
