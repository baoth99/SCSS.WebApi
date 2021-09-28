using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCSS.Data.Entities
{
    [Table("Feedback")]
    public class Feedback : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Account")]
        public Guid? SellingAccountId { get; set; }

        [ForeignKey("Account")]
        public Guid? BuyingAccountId { get; set; }
        
        [ForeignKey("SellCollectTransaction")]
        public Guid? SellCollectTransactionId { get; set; }

        [ForeignKey("CollectDealTransaction")]
        public Guid? CollectDealTransactionId { get; set; }

        public int? Type { get; set; }

        public float? Rate { get; set; }

        [MaxLength(500)]
        public string SellingReview { get; set; }

        [MaxLength(500)]
        public string SellingFeedback { get; set; }

        [MaxLength(500)]
        public string AdminReply { get; set; }

        public int? Status { get; set; }

        public bool IsDeleted { get; set; }
    }
}
