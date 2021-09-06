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

        [Column(TypeName = "VARCHAR(50)")]
        public string Code { get; set; }

        [ForeignKey("Account")]
        public Guid? DealerAccountId { get; set; }

        [ForeignKey("ScrapCategory")]
        public Guid? DealerCategoryId { get; set; }

        public long? AppliedAmount { get; set; }

        public long? BonusAmount { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public int? Status { get; set; }

        public bool IsDeleted { get; set; }
    }
}
