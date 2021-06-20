using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("TimePeriod")]
    public class TimePeriod : IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public TimePeriod TimeFrom { get; set; }

        public TimePeriod TimeTo { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}

