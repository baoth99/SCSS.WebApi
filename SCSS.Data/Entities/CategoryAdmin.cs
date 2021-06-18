using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("CategoryAdmin")]
    public class CategoryAdmin : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey("Unit")]
        public Guid? UnitId { get; set; }

        [ForeignKey("Role")]
        public Guid? RoleId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}
