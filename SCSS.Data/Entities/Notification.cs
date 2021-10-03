using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("Notification")]
    public class Notification : BaseEntity, IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Account")]
        public Guid? AccountId { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public string Body { get; set; }

        public string DataCustom { get; set; }

        public int? NotiType { get; set; }

        public bool IsRead { get; set; }

        public bool IsDeleted { get; set; }
    }
}
