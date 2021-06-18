using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("Role")]
    public class Role : IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}
