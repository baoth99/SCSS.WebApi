using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("Account")]
    public class Account
    {
        public Guid? Id { get; set; }

        public string PhoneNumber { get; set; }

        public long? TotalPoint { get; set; }
    }
}
