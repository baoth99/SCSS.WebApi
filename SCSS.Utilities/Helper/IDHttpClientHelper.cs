using Newtonsoft.Json;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class IDHttpClientHelper
    {
        private static HttpClient GetHttpClient(string clientId)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add(IdentityServer4Constant.ClientId, clientId);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationRestfulApi.ApplicationProduce));



            return httpClient;
        }

        public static async Task<HttpClientResponseModel> IDHttpClientPost(string url, string clientId, Dictionary<string, string> parameters)
        {
            var httpClient = GetHttpClient(clientId);

            var httpContent = new FormUrlEncodedContent(parameters);

            var httpResponse = await httpClient.PostAsync(url, httpContent);

            var statusCode = (int)httpResponse.StatusCode;

            if (statusCode == HttpStatusCodes.Ok)
            {
                
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<BaseApiResponseModel>(responseContent);
                var responseModel = new HttpClientResponseModel()
                {
                    StatusCode = statusCode,
                    IsSuccess = data.IsSuccess,
                    Data = data.Data,
                };              
                return responseModel;
            }
            return null;
        }
    }

    public class HttpClientResponseModel
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }
}
