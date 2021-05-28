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
    public class SearchResult
    {
        public IList<Board> Boards;
    }

    public class SearchApi : BaseApiTrello
    {

        public SearchApi()
        {
            Controller = "search";
        }

        public IList<Board> GetBoardsBySearchName(string boardName)
        {
            Client = InitializeClient();

            var request = InitializeRequest(Method.GET, Controller, string.Empty);

            request.AddQueryParameter("query", boardName);
            request.AddQueryParameter("idBoards", "mine");

            Response = Client.Execute(request);
            
            var ret = JsonConvert.DeserializeObject<SearchResult>(Response.Content);

            return ret.Boards;

        }

    }


}
