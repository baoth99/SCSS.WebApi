using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("CollectingRequest")]
    public class CollectingRequest : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string CollectingRequestCode { get; set; }

        public DateTime? CollectingRequestDate { get; set; }

        public TimeSpan? TimeFrom { get; set; }

        public TimeSpan? TimeTo { get; set; }

        [ForeignKey("Location")]
        public Guid? LocationId { get; set; }

        [ForeignKey("Account")]
        public Guid? SellerAccountId { get; set; }

        [ForeignKey("Account")]
        [ConcurrencyCheck]
        public Guid? CollectorAccountId { get; set; }

        public DateTime? ApprovedTime { get; set; }

        public bool IsBulky { get; set; }

        public string ScrapImageUrl { get; set; }

        public string Note { get; set; }

        public string CancelReason { get; set; }

        [ConcurrencyCheck]
        public int? Status { get; set; }

        public bool IsDeleted { get; set; }
    }
}

