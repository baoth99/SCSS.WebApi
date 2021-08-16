using SCSS.TwilioService.Interfaces;
using SCSS.TwilioService.Models;
using SCSS.Utilities.Configurations;
using System;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace SCSS.TwilioService.Implementations
{
    public class SMSService : TwilioBaseService, ISMSService
    {
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
            }
            catch (Exception)
            {
               // Ignore
            }     
        }
    }
}
