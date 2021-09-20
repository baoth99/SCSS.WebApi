using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCSS.Data.Entities
{
    [Table("SellCollectTransaction")]
    public class SellCollectTransaction : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("CollectingRequest")]
        public Guid? CollectingRequestId { get; set; }

        public long? Total { get; set; }

        public long? TransactionServiceFee { get; set; }

        public int? AwardPoint { get; set; }

        public bool IsDeleted { get; set; }

    }
}
