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

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }      

        public int? Gender { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        [MaxLength(256)]
        public string Address { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public DateTime? BirthDate { get; set; }

        public string ImageName { get; set; }

        [ForeignKey("Role")]
        public Guid? RoleId { get; set; }

        [MaxLength(255)]
        public string IdCard { get; set; }

        public float? TotalPoint { get; set; }

        public int? Status { get; set; }

        public string DeviceId { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
