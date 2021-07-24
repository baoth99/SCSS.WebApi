using SCSS.Utilities.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.ESMSService.Interfaces
{
    public interface ISMSService
    {
        Task SendSMS(string phone, string content, string bandName);
    }
}
