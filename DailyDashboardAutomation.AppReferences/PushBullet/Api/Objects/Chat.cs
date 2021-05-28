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
    public class Chat
    {


    }
    

    public class ChatApi : BaseApiPushBullet
    {
        public ChatApi()
        {
            Controller = "chats";
        }

    }


}
