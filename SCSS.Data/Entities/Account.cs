using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace SCSS.Data.Entities
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string DealerName { get; set; }


        [Column(TypeName = "VARCHAR(50)")]
        public string UserName { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Email { get; set; }      

        public int? Gender { get; set; }

        [Column(TypeName = "VARCHAR(20)")]
        public string Phone { get; set; }

        [MaxLength(256)]
        public string Address { get; set; }

        public DateTime? BirthDate { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        public string ImageUrl { get; set; }

        [ForeignKey("Role")]
        public Guid? RoleId { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string IdCard { get; set; }

        public float? TotalPoint { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        public string DeviceId { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
