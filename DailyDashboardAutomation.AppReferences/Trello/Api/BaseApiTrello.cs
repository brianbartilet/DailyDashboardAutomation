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

namespace AppReferences.Trello.Api
{
    public class BaseApiTrello : BaseRestApi
    {
        //https://developers.trello.com/v1.0/reference
        //https://api.trello.com/1/boards/560bf4298b3dda300c18d09c?fields=name,url&key={YOUR-API-KEY}&token={AN-OAUTH-TOKEN}

        protected override Uri BaseAddress { get; } = new Uri(ReadConfigFile.GetSettingAsString("Trello_URL"));

        //protected override Uri BaseAddress { get; } = new Uri(@"https://api.trello.com/1");
        //public string UserToken = "6c5dce0e92f3e3f3b6f43f66ee76e5f7";
        //public string Secret = "8281952326a48aa0bc6179c0ce0a322f8e7b712f79c3aed5c33dd6b1cff63aa7";

        protected override RestRequest InitializeRequest(Method method, string controller, string action, object dto=null)
        {
            var request = base.InitializeRequest(method, controller, action, dto);

            request.AddParameter("key", ReadConfigFile.GetSettingAsString("Trello_Key"));
            request.AddParameter("token", ReadConfigFile.GetSettingAsString("Trello_Token"));

            return request;
        }

        protected override RestClient InitializeClient(bool initializeCookie = false)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            return base.InitializeClient(initializeCookie);
        }
    }
}
