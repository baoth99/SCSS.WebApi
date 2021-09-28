using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.SystemConfigModels
{
    public class SystemConfigModifyModel
    {
        public int RequestQuantity { get; set; }

        public int ReceiveQuantity { get; set; }

        public int MaxNumberOfRequestDays { get; set; }
    }
}
