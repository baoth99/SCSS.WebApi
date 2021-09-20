using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("TransactionServiceFeePercent")]
    public class TransactionServiceFeePercent : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int? TransactionType { get; set; }

        public float? Percent { get; set; }

        public bool IsActive { get; set; }
    }
}
