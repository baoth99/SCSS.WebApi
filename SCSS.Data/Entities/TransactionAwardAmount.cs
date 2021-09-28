using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("TransactionAwardAmount")]
    public class TransactionAwardAmount : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int? TransactionType { get; set; }

        public long? AppliedAmount { get; set; }

        public float? Amount { get; set; }

        public bool IsActive { get; set; }
        
    }
}
