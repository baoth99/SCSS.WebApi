using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("CollectDealTransactionDetail")]
    public class CollectDealTransactionDetail : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("CollectDealTransaction")]
        public Guid? CollectDealTransactionId { get; set; }

        [ForeignKey("ScrapCategoryDetail")]
        public Guid? DealerCategoryDetailId { get; set; }

        public float? Quantity { get; set; }

        [ForeignKey("Promotion")]
        public Guid? PromotionId { get; set; }

        public long? Total { get; set; }

        public long? BonusAmount { get; set; }

        public bool IsDeleted { get; set; }

    }
}
