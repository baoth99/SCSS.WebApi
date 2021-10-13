using SCSS.Aplication.BackgroundService.Models.SMSMessageModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Interfaces
{
    public interface ISMSMessageService
    {
        /// <summary>
        /// Sends the SMS.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task SendSMS(SMSMessagePushModel model);

        /// <summary>
        /// Sends the many SMS.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns></returns>
        Task SendManySMS(List<SMSMessagePushModel> models);
    }
}
