using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCSS.Data.Entities
{
    [Table("FeedbackToSystem")]
    public class FeedbackToSystem : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int RoleFeedback { get; set; }

        [ForeignKey("Account")]
        public Guid? SellingAccountId { get; set; }

        [ForeignKey("Account")]
        public Guid? BuyingAccountId { get; set; }

        [ForeignKey("CollectDealTransaction")]
        public Guid? CollectDealTransactionId { get; set; }

        [ForeignKey("CollectingRequest")]
        public Guid? CollectingRequestId { get; set; }

        [MaxLength(500)]
        public string SellingFeedback { get; set; }

        [MaxLength(500)]
        public string AdminReply { get; set; }

        public bool IsDeleted { get; set; }
    }
}
