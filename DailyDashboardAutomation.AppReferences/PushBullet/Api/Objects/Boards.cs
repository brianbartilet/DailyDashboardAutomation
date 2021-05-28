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
    public class Board
    {
        public Board()
        {
        }

        public Board(string name)
        {
            Name = name;
        }

        public string Id;
        public string Name;
        public string Desc;
        public string DescData;
        public bool Closed;
        public string IdOrganization;
        public bool Pinned;
        public string Url;
        public string ShortUrl;
        public object Prefs;
        public object LabelNames;

    }

    public class BoardsApi : BaseApiTrello
    {
        public BoardsApi()
        {
            Controller = "boards";
        }

        public Board GetBoardByName(Board b)
        {
            var search = new SearchApi();
            var target = search.GetBoardsBySearchName(b.Name).First();

            Client = InitializeClient();

            var request = InitializeRequest(Method.GET, Controller, target.Id);

            Response = Client.Execute(request);

            return JsonConvert.DeserializeObject<Board>(Response.Content);


        }

        public IList<List> GetBoardLists(Board b)
        {
            Client = InitializeClient();

            var action = GetBoardByName(b).ShortUrl.Split('/').Last() + "/lists";

            var request = InitializeRequest(Method.GET, Controller, action);

            Response = Client.Execute(request);

            return JsonConvert.DeserializeObject<List<List>>(Response.Content);
            
        }

        public IList<Card> GetAllOpenCardsFromBoard(Board b)
        {
            Client = InitializeClient();

            var action = GetBoardByName(b).ShortUrl.Split('/').Last() + "/cards";

            var request = InitializeRequest(Method.GET, Controller, action);

            Response = Client.Execute(request);

            return JsonConvert.DeserializeObject<List<Card>>(Response.Content)
                .Where(x=>x.Closed == false).ToList();

        }

        public IList<Card> GetAllCardsOpenFromBoardList(Board b, List l)
        {
            var list = GetBoardLists(b).First(x => x.Name == l.Name);

            return GetAllOpenCardsFromBoard(b)
                .Where(x => x.IdList == list.Id).ToList();

        }

    }
    

}
