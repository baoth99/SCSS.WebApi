using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("CollectorComplaint")]
    public class CollectorComplaint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Complaint")]
        public Guid? ComplaintId { get; set; }

        [ForeignKey("Account")]
        public Guid? ComplaintedAccountId { get; set; }

        [ForeignKey("Account")]
        public Guid? CollectorAccountId { get; set; }

        [MaxLength(500)]
        public string ComplaintContent { get; set; }

        [MaxLength(500)]
        public string AdminReply { get; set; }
    }
}
