using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using AppReferences.Google.Api;

namespace AppReferences.Google.Api.Objects
{
    public class Calendar
    {
        public Calendar()
        {
        }

    }

    public class CalendarApi : BaseApiGoogle
    {
        public CalendarApi()
        {
            Controller = "calendar/v3/users/me";
        }

        public IList<List> GetCalendarInformation()
        {
            Client = InitializeClient();

            var action = "calendarList";

            var request = InitializeRequest(Method.GET, Controller, action);

            Response = Client.Execute(request);

            return JsonConvert.DeserializeObject<List<List>>(Response.Content);

        }
    }
}
