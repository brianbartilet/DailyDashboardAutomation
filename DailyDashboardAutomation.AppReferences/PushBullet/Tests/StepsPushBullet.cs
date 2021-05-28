using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using AppReferences.PushBullet.Api.Objects;

namespace AppReferences.PushBullet.Steps
{
    [Binding]
    public class StepsPushBullet
    {
        #region View SMS Messages


        #endregion

        #region Devices

        [When(@"I view my devices")]
        public void WhenIViewMyDevices()
        {
            var devicesApi = new DevicesApi();

            ScenarioContext.Current["Devices"] = devicesApi.GetDevices();
        }

        [Then(@"I can view my devices successfully")]
        public void ThenICanViewMyDevicesSuccessfully()
        {
            var devices = (List<Device>)ScenarioContext.Current["Devices"];
            Assert.Greater(devices.Count, 0);
        }

        #endregion

        #region Pushes

        [When(@"I view all my pushes")]
        public void WhenIViewAllMyPushes()
        {
            var pushesApi = new PushApi();

            ScenarioContext.Current["Pushes"] = pushesApi.GetPushes();
        }

        [Then(@"my pushes are fetched successfully")]
        public void ThenMyPushesAreFetchedSuccessfully()
        {
            var pushes = (List<Push>)ScenarioContext.Current["Pushes"];
            Assert.Greater(pushes.Count, 0);
        }


        #endregion
    }
}
