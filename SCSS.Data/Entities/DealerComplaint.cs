using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCSS.Data.Entities
{
    [Table("DealerComplaint")]
    public class DealerComplaint : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Complaint")]
        public Guid? ComplaintId { get; set; }

        [ForeignKey("Account")]
        public Guid? ComplaintedAccountId { get; set; }

        [ForeignKey("Account")]
        public Guid? DealerAccountId { get; set; }

        [MaxLength(500)]
        public string ComplaintContent { get; set; }

        [MaxLength(500)]
        public string AdminReply { get; set; }
    }
}
