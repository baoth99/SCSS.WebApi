using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("AccountCategory")]
    public class AccountCategory : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

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
        public string Status { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}
