using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("Promotion")]
    public class Promotion : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey("AccountCategory")]
        public Guid? DealerCategoryId { get; set; }

        [ForeignKey("Account")]
        public Guid? DealerAccountId { get; set; }

        public float? AppliedQuantity { get; set; }

        public float? BonusAmount { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public int? Status { get; set; }

        public bool IsDeleted { get; set; }
    }
}
