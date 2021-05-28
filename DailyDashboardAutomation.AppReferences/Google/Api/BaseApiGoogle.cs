using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppReferences.Utilities;
using RestSharp;
using System.Net;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;

namespace AppReferences.Google.Api
{
    public class BaseApiGoogle : BaseRestApi
    {
        protected override Uri BaseAddress { get; } = new Uri(ReadConfigFile.GetSettingAsString("Google_URL"));

        protected override RestRequest InitializeRequest(Method method, string controller, string action, object dto = null)
        {
            var request = new RestRequest
            {
                Method = method,
                RequestFormat = DataFormat.Json,
                Resource = controller + "/" + action,
                RootElement = "Content"
            };

            request.AddParameter("key", ReadConfigFile.GetSettingAsString("Google_Key"));

            return request;
        }

        protected override RestClient InitializeClient(bool initializeCookie = false)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            
            var client = new RestClient(BaseAddress);
            return client;
        }
    }
}
