using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("SellerComplaint")]
    public class SellerComplaint : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Account")]
        public Guid? SellingAccountId { get; set; }

        [ForeignKey("Account")]
        public Guid? BuyingAccountId { get; set; }

        [MaxLength(500)]
        public string ComplaintContent { get; set; }

        [MaxLength(500)]
        public string AdminReply { get; set; }
    }
}
