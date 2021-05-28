using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using AppReferences.PushBullet.Api;

namespace AppReferences.PushBullet.Api.Objects
{
    public class Device
    {
        public string Iden;
        public bool Active;
        public float Created;
        public float Modified;
        public string Icon;
        public string Nickname;
        public bool Generated_Nickname;
        public string Manufacturer;
        public string Model;
        public int App_Version;
        public string Fingerprint;
        public string Public_Fingerprint;
        public string Push_Token;
        public string Has_Sms;
        public string Type;
        public string Kind;
        public bool Pushable;

    }
    

    public class DevicesApi : BaseApiPushBullet
    {
        public DevicesApi()
        {
            Controller = "devices";
        }


        public IList<Device> GetDevices()
        {
            Client = InitializeClient();

            var request = InitializeRequest(Method.GET, Controller);
            
            Response = Client.Execute(request);

            var devices = JObject.Parse(Response.Content).SelectToken(Controller).ToString();

            return JsonConvert.DeserializeObject<List<Device>>(devices);

        }


    }


}
