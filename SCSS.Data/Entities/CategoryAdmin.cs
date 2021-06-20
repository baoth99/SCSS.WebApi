using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("CategoryAdmin")]
    public class CategoryAdmin : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey("Unit")]
        public Guid? UnitId { get; set; }

        [ForeignKey("Role")]
        public Guid? RoleId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool? IsLocked { get; set; }

        [ForeignKey("Account")]
        public Guid? LockedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
