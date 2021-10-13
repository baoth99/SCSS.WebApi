using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Aplication.BackgroundService.Models.SMSMessageModels;
using SCSS.TwilioService.Interfaces;
using SCSS.TwilioService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Implementations
{
    public class SMSMessageService : ISMSMessageService
    {
        #region Services

        /// <summary>
        /// The SMS service
        /// </summary>
        private readonly ISMSService _SMSService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SMSMessageService"/> class.
        /// </summary>
        /// <param name="sMSService">The s ms service.</param>
        public SMSMessageService(ISMSService sMSService)
        {
            _SMSService = sMSService;
        }

        #endregion

        #region Send SMS

        /// <summary>
        /// Sends the SMS.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task SendSMS(SMSMessagePushModel model)
        {
            if (model != null)
            {
                var sms = new SendSMSModel()
                {
                    Phone = model.Phone,
                    Content = model.Content
                };

                await _SMSService.SendSMS(sms);
            }
        }


        #endregion

        #region Send Many SMS

        /// <summary>
        /// Sends the many SMS.
        /// </summary>
        /// <param name="models">The models.</param>
        public async Task SendManySMS(List<SMSMessagePushModel> models)
        {
            if (models.Any())
            {
                var smsModels = models.Select(x => new SendSMSModel()
                {
                    Phone = x.Phone,
                    Content = x.Content
                }).ToList();

                foreach (var item in smsModels)
                {
                    await _SMSService.SendSMS(item);
                }
            }

        }

        #endregion
        
    }
}
