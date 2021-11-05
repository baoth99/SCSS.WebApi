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

        [ForeignKey("ScrapCategoryDetail")]
        public Guid? CollectorCategoryDetailId { get; set; }

        public string ScrapCategoryName { get; set; }

        public float? Quantity { get; set; }

        public long? Total { get; set; }

        public long? Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}
