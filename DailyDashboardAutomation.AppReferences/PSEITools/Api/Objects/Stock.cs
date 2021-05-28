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
using SimpleJson;
using Newtonsoft.Json.Linq;
using SimpleJson;

namespace AppReferences.PSEIStock.Api
{
    public class Stock
    {
        public Stock()
        {
        }

        public Stock(string name)
        {
            Name = name;
        }

        public string Name;
        public float Percent_Change;
        public Price Price;
        public string Symbol;
        public long Volume;

    }

    public class Price
    {
        public float Amount;
        public string Currency;
    }

    public class StockApi : BaseApiStock
    {
        public StockApi()
        {
            Controller = "stocks";
        }

        public Stock GetStockPrice(string name)
        {
            Client = InitializeClient();

            var request = InitializeRequest(Method.GET, Controller, name + ".json");

            Response = Client.Execute(request);

            var parse = JObject.Parse(Response.Content)["stock"].ToString();

            return JsonConvert.DeserializeObject<List<Stock>>(parse).First();


        }

    }
    

}
