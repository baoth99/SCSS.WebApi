using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("TransactionAwardAmount")]
    public class TransactionAwardAmount : IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public float? Amount { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedTime { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
