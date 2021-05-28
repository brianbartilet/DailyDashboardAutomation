using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using AppReferences.Trello.Api;

namespace AppReferences.Trello.Api.Objects
{
    public class List
    {
        public List()
        {
        }

        public List(string name)
        {
            Name = name;
        }

        public string Id;
        public string Name;
        public bool Closed;
        public string IdBoard;
        public float Pos;
        public bool Subscribed;

    }

    public class ListsApi : BaseApiTrello
    {
        public ListsApi()
        {
            Controller = "lists";
        }

        public void ArchiveAllCards(List dto)
        {
            Client = InitializeClient();

            var action = dto.Id.ToString() + "/archiveAllCards";

            var request = InitializeRequest(Method.POST, Controller, action);

            Response = Client.Execute<Card>(request);
                        
        }

        public void ArchiveAllCardsInAllLists(IList<List> lists)
        {
            foreach (var list in lists)
            {
                ArchiveAllCards(list);
            }

        }
    }
}
