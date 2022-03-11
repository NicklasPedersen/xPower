using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.DataStore;
using xPowerHub.Models;

namespace xPowerHub.Test.DataStore
{
    public class PowerDSTest
    {
        private PowerDS _powerDS;

        public PowerDSTest()
        {
            _powerDS = new PowerDS("./testdb.db");
        }

        [SetUp]
        public void Setup()
        {
            _powerDS.RemoveTable();
            _powerDS.AddTable();
        }

        [Test]
        public async Task SaveAsync_AddPowerUsage_ShouldReturnTrue()
        {
            Random random = new Random();
            PowerUsage powerUsage = new PowerUsage() { Taken = DateTime.Now, WattHour=random.NextDouble() * 100 };
            var saved = await _powerDS.SaveAsync(powerUsage);

            Assert.IsTrue(saved, "Was not able to add the powerUsage");
        }

        [Test]
        public async Task SaveAsync_AddPowerUsage_CouldGetIt()
        {
            Random random = new Random();
            PowerUsage? powerUsageReturn = null;
            DateTime taken = DateTime.Now;

            PowerUsage powerUsage = new PowerUsage() { Taken = taken, WattHour = random.NextDouble() * 100 };
            
            var saved = await _powerDS.SaveAsync(powerUsage);

            if(saved)
                powerUsageReturn = await _powerDS.GetAsync(taken);

            Assert.IsTrue(saved, "Was not able to add the powerUsage");
            Assert.IsTrue(powerUsageReturn != null, "Got null, should have gotton a value");
        }

        [Test]
        public async Task GetAsync_GetPowerUsage_ShouldGetIt()
        {
            Random random = new Random();
            PowerUsage? powerUsageReturn = null;
            DateTime taken = DateTime.Now;

            PowerUsage powerUsage = new PowerUsage() { Taken = taken, WattHour = random.NextDouble() * 100 };

            var saved = await _powerDS.SaveAsync(powerUsage);

            if (saved)
                powerUsageReturn = await _powerDS.GetAsync(taken);

            Assert.IsTrue(saved, "Was not able to add the powerUsage");
            Assert.IsTrue(powerUsageReturn != null, "Got null, should have gotton a value");

            if (powerUsageReturn != null)
            {
                Assert.AreEqual(powerUsageReturn.WattHour, powerUsage.WattHour, "Got the wrong watthour value");
                Assert.AreEqual(powerUsageReturn.Taken, powerUsage.Taken, "Got the wrong date value");
            }
        }

        [Test]
        public async Task GetAsync_GetUnkownPowerUsage_ShouldGetNull()
        {
            Random random = new Random();
            PowerUsage? powerUsageReturn = null;
            DateTime taken = DateTime.Now;

            PowerUsage powerUsage = new PowerUsage() { Taken = taken, WattHour = random.NextDouble() * 100 };

            var saved = await _powerDS.SaveAsync(powerUsage);

            if (saved)
                powerUsageReturn = await _powerDS.GetAsync(taken.AddDays(32));

            Assert.IsTrue(saved, "Was not able to add the powerUsage");
            Assert.IsTrue(powerUsageReturn == null, "Got value from a unknown date");
        }

        [Test]
        public async Task GetPowerUsageHourlyAvgAsync_GetItFromOneHour_ShouldGetIt()
        {
            Random random = new();
            List<PowerUsage> powerUsageReturn = new();
            DateTime taken = DateTime.Now;
            double wattHour = random.NextDouble() * 100;

            PowerUsage powerUsage = new PowerUsage() { Taken = taken, WattHour = wattHour };

            var saved = await _powerDS.SaveAsync(powerUsage);

            if (saved)
            {
                powerUsageReturn.AddRange(await _powerDS.GetPowerUsageHourlyAvgAsync(taken));
            }

            Assert.IsTrue(saved, "Was not able to add the powerUsage");
            Assert.IsTrue(powerUsageReturn.Count > 0, "Did not get a value");
            Assert.AreEqual(powerUsageReturn[0].WattHour, wattHour, "The wattHour was not the right value");
        }

        [Test]
        public async Task GetPowerUsageHourlyAvgAsync_GetItFromMultipleHours_ShouldGetThem()
        {
            Random random = new();
            List<PowerUsage> powerUsageReturn = new();
            DateTime taken = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            int NumberOfHours = random.Next(1,25);
            double[] wattHours = new double[NumberOfHours];
            bool[] saves = new bool[NumberOfHours];

            for (int i = 0; i < NumberOfHours; i++)
            {
                wattHours[i] = random.NextDouble() * 100;
                var powerUsage = new PowerUsage() { Taken = taken.AddHours(i), WattHour = wattHours[i] };
                saves[i] = await _powerDS.SaveAsync(powerUsage);

            }

            powerUsageReturn.AddRange(await _powerDS.GetPowerUsageHourlyAvgAsync(taken));

            for (int i = 0; i < NumberOfHours; i++)
            {
                Assert.IsTrue(saves[i], "Was not able to add the powerUsage");
            }

            Assert.IsTrue(powerUsageReturn.Count == NumberOfHours, "Did not get all the values");

            for (int i = 0; i < NumberOfHours; i++)
            {
                Assert.AreEqual(powerUsageReturn[i].WattHour, wattHours[i], "The wattHour was not the right value");
                Assert.IsTrue(saves[i], "Was not able to add the powerUsage");
            }
        }

        [Test]
        public async Task GetPowerUsageHourlyAvgAsync_GetFromNothing_ShouldGetEmpty()
        {
            List<PowerUsage> powerUsageReturn = new();
            DateTime taken = DateTime.Now;

            powerUsageReturn.AddRange(await _powerDS.GetPowerUsageHourlyAvgAsync(taken));

            Assert.IsTrue(powerUsageReturn.Count == 0, "Did get some value");
        }

        [Test]
        public async Task GetPowerUsageWeekdayAvgAsync_GetFromNothing_ShouldGetEmpty()
        {
            List<PowerUsage> powerUsageReturn = new();

            powerUsageReturn.AddRange(await _powerDS.GetPowerUsageWeekdayAvgAsync());

            Assert.IsTrue(powerUsageReturn.Count == 0, "Did get some value");
        }

        [Test]
        public async Task GetPowerUsageWeekdayAvgAsync_GetAvgFromOne_ShouldGetavg()
        {
            Random random = new();
            List<PowerUsage> powerUsageReturn = new();
            DateTime taken = DateTime.Now;
            double wattHour = random.NextDouble() * 100;

            PowerUsage powerUsage = new PowerUsage() { Taken = taken, WattHour = wattHour };

            var saved = await _powerDS.SaveAsync(powerUsage);

            if (saved)
            {
                powerUsageReturn.AddRange(await _powerDS.GetPowerUsageWeekdayAvgAsync());
            }

            Assert.IsTrue(saved, "Was not able to add the powerUsage");
            Assert.IsTrue(powerUsageReturn.Count > 0, "Did not get a value");
            Assert.AreEqual(powerUsageReturn[0].WattHour, wattHour, "The wattHour was not the right value");
        }

        [Test]
        public async Task GetPowerUsageWeekdayAvgAsync_GetAvgFromMultiple_ShouldGetavg()
        {
            Random random = new();
            List<PowerUsage> powerUsageReturn = new();
            DateTime taken = DateTime.Now;
            int NumberOfWeeks = random.Next(1, 25);
            double[] wattHours = new double[NumberOfWeeks];
            bool[] saves = new bool[NumberOfWeeks];

            for (int i = 0; i < NumberOfWeeks; i++)
            {
                wattHours[i] = random.NextDouble() * 100;
                var powerUsage = new PowerUsage() { Taken = taken.AddDays(i*7), WattHour = wattHours[i] };
                saves[i] = await _powerDS.SaveAsync(powerUsage);

            }

            powerUsageReturn.AddRange(await _powerDS.GetPowerUsageWeekdayAvgAsync());

            for (int i = 0; i < NumberOfWeeks; i++)
            {
                Assert.IsTrue(saves[i], "Was not able to add the powerUsage");
            }

            Assert.IsTrue(powerUsageReturn.Count == 1, "Did get multiple values");

            var avg = wattHours.Sum() / NumberOfWeeks;
            Assert.AreEqual(powerUsageReturn[0].WattHour, avg, "The wattHour was not the right value");
        }

        [Test]
        public async Task UpdateAsync_UpdateWatthour_ShouldReturnTrue()
        {
            Random random = new Random();
            DateTime taken = DateTime.Now;
            double newWattHour = random.NextDouble() * 100;
            bool updated = false;
            bool saved;

            PowerUsage powerUsage = new PowerUsage() { Taken = taken, WattHour = random.NextDouble() * 100 };

            saved = await _powerDS.SaveAsync(powerUsage);

            if (saved)
            {
                powerUsage.WattHour = newWattHour;
                updated = await _powerDS.UpdateAsync(powerUsage);
            }

            Assert.IsTrue(saved, "Was not able to add the powerUsage");
            Assert.IsTrue(updated, "Was not able to update the powerUsage");
        }

        [Test]
        public async Task UpdateAsync_UpdateWatthour_ShouldGetIt()
        {
            Random random = new Random();
            PowerUsage? powerUsageReturn = null;
            DateTime taken = DateTime.Now;
            double newWattHour = random.NextDouble() * 100;
            bool updated = false;
            bool saved;

            PowerUsage powerUsage = new PowerUsage() { Taken = taken, WattHour = random.NextDouble() * 100 };

            saved = await _powerDS.SaveAsync(powerUsage);

            if (saved)
            {
                powerUsage.WattHour = newWattHour;
                updated = await _powerDS.UpdateAsync(powerUsage);
                if (updated)
                    powerUsageReturn = await _powerDS.GetAsync(taken);
            }

            Assert.IsTrue(saved, "Was not able to add the powerUsage");
            Assert.IsTrue(updated, "Was not able to update the powerUsage");

            if (powerUsageReturn != null)
            {
                Assert.AreEqual(powerUsageReturn.WattHour, powerUsage.WattHour, "Got the wrong watthour value");
                Assert.AreEqual(powerUsageReturn.Taken, powerUsage.Taken, "Got the wrong date value");
            }
        }
    }
}
