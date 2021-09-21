using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.TransactionServiceFeeModels
{
    public class TransactionServiceFeeCreateModel
    {
        public int TransactionType { get; set; }

        public float Percent { get; set; }
    }
}
