using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("CollectingRequestRejection")]
    public class CollectingRequestRejection : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("CollectingRequest")]
        public Guid? CollectingRequestId { get; set; }

        [ForeignKey("Account")]
        public Guid? CollectorId { get; set; }

        [MaxLength(255)]
        public string Reason { get; set; }

        public bool IsDeleted { get; set; }
    }
}
