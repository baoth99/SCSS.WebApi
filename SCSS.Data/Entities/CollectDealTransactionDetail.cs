using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("CollectDealTransactionDetail")]
    public class CollectDealTransactionDetail : IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        [ForeignKey("CollectDealTransaction")]
        public Guid? CollectDealTransactionId { get; set; }

        [ForeignKey("AccountCategory")]
        public Guid? DealerCategoryId { get; set; }

        public float? Quantity { get; set; }

        [ForeignKey("Promotion")]
        public Guid? PromotionId { get; set; }

        public float? Total { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}
