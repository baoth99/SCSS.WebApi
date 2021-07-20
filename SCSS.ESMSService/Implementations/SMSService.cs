using SCSS.ESMSService.Interfaces;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SCSS.ESMSService.Implementations
{
    public class SMSService : ISMSService
    {
        public async Task SendSMS(string phone, string content, string bandName)
        {
            string requestUrl = $"{AppSettingValues.ESMSApiUrl}?"+
                            $"Phone={phone}&Content={content}&"+
                            $"ApiKey={AppSettingValues.ESMSApiKey}&"+
                            $"SecretKey={AppSettingValues.ESMSApiSecret}&"+
                            $"IsUnicode=0&Brandname={bandName}&SmsType=2";

            Uri address = new Uri(requestUrl);

            HttpWebRequest request;
            HttpWebResponse response = null;
            StreamReader reader;

            if (address == null) { 
                throw new ArgumentNullException("Address is Null"); 
            }

            try
            {
                request = WebRequest.Create(address) as HttpWebRequest;
                request.UserAgent = ".NET Sample";
                request.KeepAlive = BooleanConstants.FALSE;
                request.Timeout = 15 * 1000;
                response = await request.GetResponseAsync() as HttpWebResponse;
                if (request.HaveResponse && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    string result = reader.ReadToEnd();
                    result = result.Replace("</string>", "");
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        Console.WriteLine(
                            "The server returned '{0}' with the status code {1} ({2:d}).",
                            errorResponse.StatusDescription, errorResponse.StatusCode,
                            errorResponse.StatusCode);
                    }
                }
            }
            finally
            {
                if (response != null) 
                { 
                    response.Close(); 
                }
            }
        }
    }
}
