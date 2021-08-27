using SCSS.AWSService.Interfaces;
using SCSS.TwilioService.Interfaces;
using SCSS.TwilioService.Models;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using System;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace SCSS.TwilioService.Implementations
{
    public class SMSService : TwilioBaseService, ISMSService
    {
        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="SMSService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SMSService(ILoggerService logger) : base(logger)
        {
        }

        #endregion

        #region Send SMS

        /// <summary>
        /// Sends the SMS.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task SendSMS(SendSMSModel model)
        {
            var phoneNumberFrom = AppSettingValues.TwilioPhoneNumber;
            var phoneNumberTo = "+84" + model.Phone.Substring(1);

            try
            {
                await MessageResource.CreateAsync(
                    body: model.Content,
                    from: new Twilio.Types.PhoneNumber(phoneNumberFrom),
                    to: new Twilio.Types.PhoneNumber(phoneNumberTo)
                );
                Logger.LogInfo(TwilioLoggerMessage.SendSMSSuccess(model.Content, phoneNumberTo));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, TwilioLoggerMessage.SendSMSFail(phoneNumberTo));
            }
        }

        #endregion

    }
}
