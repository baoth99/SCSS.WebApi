using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("DealerInformation")]
    public class DealerInformation : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string DealerName { get; set; }

        [ForeignKey("Account")]
        public Guid? DealerAccountId { get; set; }

        public string DealerImageUrl { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string DealerPhone { get; set; }

        [ForeignKey("Location")]
        public Guid? LocationId { get; set; }

        public TimeSpan? OpenTime { get; set; }

        public TimeSpan? CloseTime { get; set; }

        public float Rating { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
