using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using AppReferences.Google.Api.Objects;

namespace AppReferences.Google.Tests
{
    [Binding]
    public sealed class GoogleSteps
    {

        [When(@"I view all (available) calendars")]
        public void WhenIViewAllAvailableCalendars(string status)
        {
            var calendarApi = new CalendarApi();

            switch (status)
            {
                case "available":
                case "open":
                    ScenarioContext.Current["Cards"] =
                        calendarApi.GetCalendarInformation();
                    break;
                case "closed":
                    throw new NotImplementedException();
                case "archived":
                    throw new NotImplementedException();
                case "listed":

                    break;
                default:
                    throw new NotImplementedException();

            }
        }

        [Then(@"I can view all my (available) cards")]
        public void ThenICanViewAllMyAvailableCards(string status)
        {
            switch (status)
            {
                case "existing":
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

    }
}
