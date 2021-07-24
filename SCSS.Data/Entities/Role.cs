using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SCSS.Data.Entities
{
    [Table("Role")]
    public class Role : IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int Key { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

    }
}
