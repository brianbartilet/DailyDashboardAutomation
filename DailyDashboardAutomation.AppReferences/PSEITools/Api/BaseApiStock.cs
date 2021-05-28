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

namespace AppReferences.PSEIStock.Api
{
    public class BaseApiStock : BaseRestApi
    {
        protected override Uri BaseAddress { get; } = new Uri(ReadConfigFile.GetSettingAsString("PSEI_API_URL"));

        protected override RestRequest InitializeRequest(Method method, string controller, string action, object dto=null)
        {
            var request = base.InitializeRequest(method, controller, action, dto);


            return request;
        }

        protected override RestClient InitializeClient(bool initializeCookie = false)
        {
            return base.InitializeClient(initializeCookie);
        }
    }
}
