using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCSS.Data.Entities
{
    [Table("PersonalSellerLocation")]
    public class PersonalSellerLocation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Account")]
        public Guid? SellerAccountId { get; set; }

        public string PlaceId { get; set; }

        public string PlaceName { get; set; }

        [ForeignKey("Location")]
        public Guid? LocationId { get; set; }
    }
}
