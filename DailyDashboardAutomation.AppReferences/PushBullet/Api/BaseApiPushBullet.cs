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

namespace AppReferences.PushBullet.Api
{
    public class BaseApiPushBullet : BaseRestApi
    {
        //https://docs.pushbullet.com/#pushbullet-api

        protected override Uri BaseAddress { get; } = new Uri(ReadConfigFile.GetSettingAsString("Pushbullet_URL"));

        protected override RestRequest InitializeRequest(Method method, string controller, string action=null, object dto=null)
        {
            var request = base.InitializeRequest(method, controller, action, dto);

            request.AddHeader("Access-Token", ReadConfigFile.GetSettingAsString("Pushbullet_Token"));

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
