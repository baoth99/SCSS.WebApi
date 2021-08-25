using SCSS.TwilioService.Models;
using System.Threading.Tasks;

namespace SCSS.TwilioService.Interfaces
{
    public interface ISMSService
    {
        /// <summary>
        /// Sends the SMS.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task SendSMS(SendSMSModel model);
    }
}
