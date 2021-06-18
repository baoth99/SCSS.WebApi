using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("Booking")]
    public class Booking : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        [ForeignKey("ScheduleType")]
        public Guid? ScheduleTypeId { get; set; }

        [ForeignKey("TimeSpan")]
        public Guid? TimeSpanId { get; set; }

        [ForeignKey("ItemType")]
        public Guid? ItemTypeId { get; set; }

        [ForeignKey("Location")]
        public Guid? LocationId { get; set; }

        [ForeignKey("Account")]
        public Guid? SellerAccountId { get; set; }

        [ForeignKey("Account")]
        public Guid? CollectorAccountId { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}

