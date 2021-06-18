using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("SellCollectTransaction")]
    public class SellCollectTransaction : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }


        [ForeignKey("Booking")]
        public Guid? BookingId { get; set; }

        public float? Total { get; set; }

        public long? AwardPoint { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}
