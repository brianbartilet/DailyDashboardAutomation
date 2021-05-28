using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using AppReferences.Trello.Api.Objects;

namespace AppReferences.Trello.Steps
{
    [Binding]
    public class StepsTrello
    {
        #region View Cards

        [Given(@"I a have an (existing|closed|archived) board")]
        public void GivenIaHaveAnExistingBoard(string status)
        {
            switch (status)
            {
                case "existing":
                    ScenarioContext.Current["Board"] = new Board("Daily Dashboard Tests");
                    break;
                case "closed":
                    throw new NotImplementedException();
                case "archived":
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();

            }

        }

        [Given(@"I have an (existing|closed|archived|non-existing) list")]
        public void GivenIHaveAnExistingList(string status)
        {
            switch (status)
            {
                case "existing":
                    ScenarioContext.Current["List"] = new Api.Objects.List("CURRENT");
                    break;
                case "non-existing":
                    throw new NotImplementedException();
                    break;
                case "closed":
                    throw new NotImplementedException();
                case "archived":
                    throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();

            }

        }

        [When(@"I view all (open|available|closed|archived|listed) cards")]
        public void WhenIViewAllAvailableCards(string status)
        {
            var boardsApi = new BoardsApi();

            switch (status)
            {
                case "available":
                case "open":
                    ScenarioContext.Current["Cards"] =
                        boardsApi.GetAllOpenCardsFromBoard((Board)ScenarioContext.Current["Board"]);
                    break;
                case "closed":
                    throw new NotImplementedException();
                case "archived":
                    throw new NotImplementedException();
                case "listed":
                    ScenarioContext.Current["Cards"] =
                        boardsApi.GetAllCardsOpenFromBoardList((Board)ScenarioContext.Current["Board"],
                            (Api.Objects.List)ScenarioContext.Current["List"]);
                    break;
                default:
                    throw new NotImplementedException();

            }

        }

        [Then(@"I can view all my cards")]
        public void ThenICanViewAllMyCards()
        {
            var cardList = (List<Card>)ScenarioContext.Current["Cards"];

            Assert.GreaterOrEqual(cardList.ToList().Count, 0);
        }

        #endregion

        #region Add Cards


        [When(@"I add a card with the following items to the (.*) of list")]
        public void WhenIAddACardWithTheFollowingItemsToTopOfList(string pos)
        {
            var targetBoard = (Board)ScenarioContext.Current["Board"];
            var targetList = (Api.Objects.List)ScenarioContext.Current["List"];

            var cardsApi = new CardsApi();
            var boardsApi = new BoardsApi();

            var card = new Card();

            try
            {
                card.Name = "New Automation Card " + Guid.NewGuid();
                card.IdList = boardsApi.GetBoardLists(targetBoard).First(x => x.Name == targetList.Name).Id;
                card.Pos = "bottom";
            }
            catch (Exception)
            {
                card.Name = "New Automation Card " + Guid.NewGuid();
                card.IdList = targetList.Name; //make it really invalid
                card.Pos = "bottom";
            }

            var newCard = cardsApi.AddCard(card);

            ScenarioContext.Current["Card"] = newCard;
        }

        [Then(@"my card is added (unsuccessfully|successfully)")]
        public void ThenMyCardIsAddedSuccessfully(string result)
        {
            var targetCard = (Card)ScenarioContext.Current["Card"];

            var boardsApi = new BoardsApi();

            switch (result)
            {
                case "unsuccessfully":
                    var countBoard = boardsApi.GetAllOpenCardsFromBoard((Board)ScenarioContext.Current["Board"])
                        .Count(x => x.Name == targetCard.Name);
                    Assert.AreEqual(countBoard, 0);
                    break;
                case "successfully":
                    var count = boardsApi.GetAllCardsOpenFromBoardList((Board)ScenarioContext.Current["Board"], (Api.Objects.List)ScenarioContext.Current["List"])
                        .Count(x => x.Name == targetCard.Name);
                    Assert.GreaterOrEqual(count, 1);
                    break;
                default:
                    throw new NotImplementedException();
            }

        }

        #endregion

        #region Move Cards

        [When(@"I move the card to another list")]
        public void WhenIMoveTheCardToAnotherList()
        {       
            var boardsApi = new BoardsApi();
            var cardsApi = new CardsApi();
            Board targetBoard = (Board)ScenarioContext.Current["Board"];
            Api.Objects.List moveToList = new Api.Objects.List("COMPLETED");

            ScenarioContext.Current["List"] = moveToList;
            var targetListId = boardsApi.GetBoardLists(targetBoard).First(x => x.Name == moveToList.Name).Id;
            
            var targetCard = (Card)ScenarioContext.Current["Card"];

            var updateCard = new Card()
            {
                Id = targetCard.Id,
                IdList = targetListId,
                Name = "Moved " + targetCard.Name

            };

            ScenarioContext.Current["Card"] = cardsApi.UpdateCard(updateCard);

        }

        [Then(@"the card is moved successfully")]
        public void ThenTheCardIsMovedSuccessfully()
        {
            var boardsApi = new BoardsApi();
            var targetCard = (Card)ScenarioContext.Current["Card"];
            var count = boardsApi.GetAllCardsOpenFromBoardList((Board)ScenarioContext.Current["Board"], (Api.Objects.List)ScenarioContext.Current["List"])
                        .Count(x => x.Name == targetCard.Name);
            Assert.GreaterOrEqual(count, 1);

        }

        #endregion

        #region Archive Cards

        [When(@"I archive the card")]
        public void WhenIArchiveTheCard()
        {
            var boardsApi = new BoardsApi();
            var cardsApi = new CardsApi();
            Board targetBoard = (Board)ScenarioContext.Current["Board"];
            Api.Objects.List moveToList = new Api.Objects.List("COMPLETED");

            ScenarioContext.Current["List"] = moveToList;
            var targetListId = boardsApi.GetBoardLists(targetBoard).First(x => x.Name == moveToList.Name).Id;

            var targetCard = (Card)ScenarioContext.Current["Card"];

            var updateCard = new Card()
            {
                Id = targetCard.Id,
                Closed = true

            };

            ScenarioContext.Current["Card"] = cardsApi.UpdateCard(updateCard);
        }

        [Then(@"the card is archived successfully")]
        public void ThenTheCardIsArchivedSuccessfully()
        {
            var boardsApi = new BoardsApi();
            var targetCard = (Card)ScenarioContext.Current["Card"];
            var count = boardsApi.GetAllCardsOpenFromBoardList((Board)ScenarioContext.Current["Board"], (Api.Objects.List)ScenarioContext.Current["List"])
                        .Count(x => x.Name == targetCard.Name);
            Assert.GreaterOrEqual(count, 0);
        }

        [When(@"I archive all cards in the list")]
        public void WhenIArchiveAllCardsInTheList()
        {   var boardsApi = new BoardsApi();
            var targetList = (Api.Objects.List)ScenarioContext.Current["List"];
            var useList = boardsApi.GetBoardLists((Board)ScenarioContext.Current["Board"])
                .First(x => x.Name == targetList.Name);

            var listsApi = new ListsApi();
            listsApi.ArchiveAllCards(useList);

        }

        [Then(@"all cards are archived")]
        public void ThenAllCardsAreArchived()
        {
            var boardsApi = new BoardsApi();
            var count = boardsApi.GetAllCardsOpenFromBoardList((Board)ScenarioContext.Current["Board"], (Api.Objects.List)ScenarioContext.Current["List"]).Count();
            Assert.AreEqual(count, 0);
        }

        [When(@"I archive all cards in the board")]
        public void WhenIArchiveAllCardsInTheBoard()
        {
            var boardsApi = new BoardsApi();
            var lists = boardsApi.GetBoardLists((Board)ScenarioContext.Current["Board"]);

            var listsApi = new ListsApi();
            listsApi.ArchiveAllCardsInAllLists(lists);

        }

        [Then(@"all cards are archived in the board")]
        public void ThenAllCardsAreArchivedInTheBoard()
        {
            var boardsApi = new BoardsApi();
            var lists = boardsApi.GetBoardLists((Board)ScenarioContext.Current["Board"]);

            foreach (var list in lists)
            {
                var count = boardsApi.GetAllCardsOpenFromBoardList((Board)ScenarioContext.Current["Board"], list).Count();
                Assert.AreEqual(count, 0);
            }
        }

        #endregion

        #region View Board Items

        [When(@"I get all labels from the board")]
        public void WhenIGetAllLabelsFromTheBoard()
        {
            var board = (Board)ScenarioContext.Current["Board"];
            var boardsApi = new BoardsApi();

            var labels = boardsApi.GetLabelsFromBoard(board);

            ScenarioContext.Current["Labels"] = labels;

        }

        [Then(@"the labels are fetched successfully")]
        public void ThenTheLabelsAreFetchedSuccessfully()
        {
            var labels = (List<Label>)ScenarioContext.Current["Labels"];

            Assert.Greater(labels.Count, 0);
        }

        #endregion


    }
}
