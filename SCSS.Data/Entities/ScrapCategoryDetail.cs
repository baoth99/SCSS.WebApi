using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCSS.Data.Entities
{
    [Table("ScrapCategoryDetail")]
    public class ScrapCategoryDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("ScrapCategory")]
        public Guid? ScrapCategoryId { get; set; }

        public string Unit { get; set; }

        public long? Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}
