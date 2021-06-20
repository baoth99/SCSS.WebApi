using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("AccountCategory")]
    public class AccountCategory : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey("Account")]
        public Guid? AccountId { get; set; }

        [ForeignKey("Unit")]
        public Guid? UnitId { get; set; }

        [ForeignKey("CategoryAdmin")]
        public Guid? CategoryAdminId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(20)]
        public int Status { get; set; }

        public bool IsDeleted { get; set; }

    }
}
