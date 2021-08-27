using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("ServiceTransaction")]
    public class ServiceTransaction : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Account")]
        public Guid? CollectorId { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? Period { get; set; }

        public bool IsDeleted { get; set; }
    }
}
