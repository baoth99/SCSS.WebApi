using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCSS.Data.Entities
{
    [Table("ScrapCategory")]
    public class ScrapCategory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey("Account")]
        public Guid? AccountId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
