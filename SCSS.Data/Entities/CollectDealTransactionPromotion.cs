using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("CollectDealTransactionPromotion")]
    public class CollectDealTransactionPromotion : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Promotion")]
        public Guid? PromotionId { get; set; }

        [ForeignKey("CollectDealTransactionDetail")]
        public Guid? CollectDealTransactionDetailId { get; set; }

        public decimal? BonusAmount { get; set; }

        public bool IsDeleted { get; set; }
    }
}
