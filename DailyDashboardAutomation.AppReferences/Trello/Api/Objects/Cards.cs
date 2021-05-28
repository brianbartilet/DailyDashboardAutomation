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
    public class Card
    {
        public string Id;
        public object Badges;
        public object CheckItemStates;
        public bool Closed;
        public object DateLastActivity;
        public string Desc;
        public object DescData;
        public object Due;
        public bool DueComplete;
        public string Email;
        public string IdAttachmentCover;
        public string IdBoard;
        public string[] IdChecklists;
        public string[] IdLabels;
        public string[] IdMembers;
        public string[] IdMembersVoted;
        public int IdShort;
        public string IdList;
        public string Name;
        public string Pos;
        public string ShortLink;
        public string ShortUrl;
        public string Url;
        public bool Subscribed;
        public bool ManualCoverAttachment;
    }

    public class CardsApi : BaseApiTrello
    {
        public CardsApi()
        {   
            Controller = "cards";
        }

        public Card AddCard(Card dto)
        {
            Client = InitializeClient();

            var request = InitializeRequest(Method.POST, Controller, string.Empty, dto);

            Response = Client.Execute<Card>(request);

            return JsonConvert.DeserializeObject<Card>(Response.Content);
        }

        public Card UpdateCard(Card dto)
        {
            Client = InitializeClient();

            var request = InitializeRequest(Method.PUT, Controller, dto.Id.ToString(), dto);

            Response = Client.Execute<Card>(request);

            return JsonConvert.DeserializeObject<Card>(Response.Content);
        }

    }


}
