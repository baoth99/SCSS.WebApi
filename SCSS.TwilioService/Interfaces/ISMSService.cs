using SCSS.TwilioService.Models;
using System.Threading.Tasks;

namespace SCSS.TwilioService.Interfaces
{
    public interface ISMSService
    {
        Task SendSMS(SendSMSModel model);
    }
}
