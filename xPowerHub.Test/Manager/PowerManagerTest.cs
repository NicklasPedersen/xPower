using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.DataStore;
using xPowerHub.Managers;
using xPowerHub.Models;

namespace xPowerHub.Test.Manager
{
    internal class PowerManagerTest
    {
        private PowerManager _powerManager;
        private PowerDS _powerDS;

        public PowerManagerTest()
        {
            _powerDS = new PowerDS("./testdb.db");
            _powerManager = new PowerManager(_powerDS);
        }

        [SetUp]
        public void Setup()
        {
            _powerDS.RemoveTable();
            _powerDS.AddTable();
        }

        [Test]
        public async Task AddUsageAsync_AddSingle_ShouldReturnTrue()
        {
            Random random = new Random();
            PowerUsage powerUsage = new PowerUsage() { Taken = DateTime.Now, WattHour = random.NextDouble()*100 };
            bool saved;

            saved = await _powerManager.AddUsageAsync(powerUsage);

            Assert.IsTrue(saved, "Was not able to save it");
        }

        [Test]
        public async Task AddUsageAsync_AddMultipleWithinOneHour_ShouldReturnTrues()
        {
            Random random = new Random();
            int amount = random.Next(1,25);
            bool[] saved = new bool[amount];

            for (int i = 0; i < amount; i++)
            {
                saved[i] = await _powerManager.AddUsageAsync(new PowerUsage() { Taken = DateTime.Now.AddSeconds(i), WattHour = random.NextDouble() * 100 });
            }

            for (int i = 0; i < amount; i++)
            {
                Assert.IsTrue(saved[i], "Was not able to save it");
            }
        }

        [Test]
        public async Task GetpowerUsageTodayAvgAsync_GetItFromOne_ShouldReturnTheWattHour()
        {
            Random random = new Random();
            PowerUsage powerUsage = new PowerUsage() { Taken = DateTime.Now, WattHour = random.NextDouble() * 100 };
            double returnValue;
            bool saved;

            saved = await _powerManager.AddUsageAsync(powerUsage);

            returnValue = await _powerManager.GetpowerUsageTodayAvgAsync();

            Assert.IsTrue(saved, "Was not able to save it");
            Assert.AreEqual(returnValue, powerUsage.WattHour, "Did not Get the same watthour");
        }

        [Test]
        public async Task GetpowerUsageTodayAvgAsync_GetItFromMultiple_ShouldReturnTheSum()
        {
            Random random = new Random();
            int amount = random.Next(1, 25);
            double[] wattHours = new double[amount];
            bool[] saved = new bool[amount];
            double returnValue;

            for (int i = 0; i < amount; i++)
            {
                wattHours[i] = random.NextDouble()*10;
                saved[i] = await _powerManager.AddUsageAsync(new PowerUsage() { Taken = DateTime.Now.AddSeconds(i), WattHour = wattHours[i] });
            }

            returnValue = await _powerManager.GetpowerUsageTodayAvgAsync();

            for (int i = 0; i < amount; i++)
            {
                Assert.IsTrue(saved[i], "Was not able to save it");
            }

            var sum = wattHours.Sum();
            Assert.AreEqual(returnValue, sum, "Did not get the sum of the power usage");
        }

        [Test]
        public async Task GetpowerUsageTodayAvgAsync_GetItFromNothing_ShouldReturnZero()
        {
            double returnValue;

            returnValue = await _powerManager.GetpowerUsageTodayAvgAsync();

            Assert.AreEqual(returnValue, 0, "Did not get zero");
        }
    }
}
