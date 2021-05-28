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
    public class Push
    {
        public string Iden;
        public bool Active;
        public float Created;
        public float Modified;
        public string Type;
        public bool Dismissed;
        public string Guid;
        public string Direction;
        public string Sender_Iden;
        public string Sender_Email;
        public string Sender_Email_Normalized;
        public string Target_Device_Iden;
        public string Source_Device_Iden;
        public string Client_Iden;
        public string Channel_Iden;
        public string [] Awake_App_Guids;
        public string Title;
        public string Body;
        public string Url;
        public string File_Name;
        public string File_Type;
        public string File_Url;
        public string Image_Url;
        public string Image_Width;
        public string Image_Height;

    }
    

    public class PushApi : BaseApiPushBullet
    {
        public PushApi()
        {
            Controller = "pushes";
        }


        public IList<Push> GetPushes()
        {
            Client = InitializeClient();

            var request = InitializeRequest(Method.GET, Controller);
            
            Response = Client.Execute(request);

            var pushes = JObject.Parse(Response.Content).SelectToken(Controller).ToString();

            return JsonConvert.DeserializeObject<List<Push>>(pushes);

        }


    }


}
