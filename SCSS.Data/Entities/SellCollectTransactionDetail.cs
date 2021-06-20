using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("SellCollectTransactionDetail")]
    public class SellCollectTransactionDetail : IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("SellCollectTransaction")]
        public Guid? SellCollectTransactionId { get; set; }

        [ForeignKey("AccountCategory")]
        public Guid? CollectorCategoryId { get; set; }

        public float? Quantity { get; set; }

        public decimal? Total { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}
