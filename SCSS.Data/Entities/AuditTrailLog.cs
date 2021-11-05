using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("AuditTrailLog")]
    public class AuditTrailLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? AccountId { get; set; }

        public DateTime? DateTimeStamp { get; set; }

        [StringLength(255)]
        public string TableName { get; set; }

        [StringLength(255)]
        public string ChangeColumns { get; set; }

        public string State { get; set; }

        public Guid RecordId { get; set; }

        public string OldData { get; set; }

        public string NewData { get; set; }

        public string Reason { get; set; }

        public string OtherData { get; set; }

    }
}
