using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.ESMSService.Models
{
    public class SendSMSResponseModel
    {
        public string CodeResult { get; set; }

        public string CountRegenerate { get; set; }

        public string ErrorMessage { get; set; }
    }
}
