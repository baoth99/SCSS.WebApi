using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("TransactionServiceFeePercent")]
    public class TransactionServiceFeePercent : IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int? TransactionType { get; set; }

        public float? Percent { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedTime { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
