using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.Entities
{
    [Table("SellCollectTransactionDetail")]
    public class SellCollectTransactionDetail : IHasSoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        [ForeignKey("SellCollectTransaction")]
        public Guid? SellCollectTransactionId { get; set; }

        [ForeignKey("AccountCategory")]
        public Guid? CollectorCategoryId { get; set; }

        public float? Quantity { get; set; }

        public float? Total { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}
