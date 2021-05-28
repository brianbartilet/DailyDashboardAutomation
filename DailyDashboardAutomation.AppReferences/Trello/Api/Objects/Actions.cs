using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using AppReferences.Trello.Api;

namespace AppReferences.Trello.Api.Objects
{
    public class Action
    {
        public string Id;
        public object Data;
        public object Date;
        public string IdMemberCreator;
        public string Type;
    }
    

    public class ActionsApi : BaseApiTrello
    {
        public ActionsApi()
        {
            Controller = "actions";
        }

    }


}
