using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }      

        public bool? Gender { get; set; }

        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        [MaxLength(256)]
        public string Address { get; set; }

        [MaxLength(256)]
        public string Latitude { get; set; }

        [MaxLength(256)]
        public string Longitude { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Image { get; set; }

        [ForeignKey("Role")]
        public Guid? RoleId { get; set; }

        [MaxLength(20)]
        public string IDCard { get; set; }

        public long? TotalPoint { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
