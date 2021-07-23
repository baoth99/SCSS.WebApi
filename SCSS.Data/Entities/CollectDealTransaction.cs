using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("CollectDealTransaction")]
    public class CollectDealTransaction : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string TransactionCode { get; set; }

        [ForeignKey("Account")]
        public Guid? DealerAccountId { get; set; }

        [ForeignKey("Account")]
        public Guid? CollectorAccountId { get; set; }

        public decimal? Total { get; set; }

        public decimal? BonusAmount { get; set; }

        public float? AwardPoint { get; set; }

        public bool IsDeleted { get; set; }
    }
}
