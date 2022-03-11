using Microsoft.Data.Sqlite;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.DataStore;

namespace xPowerHub.Test.DataStore
{
    internal class SmartThingsDSTest
    {
        private SmartThingsDS ds;
        public SmartThingsDSTest()
        {
            ds = new("testdb.db");
        }
        [SetUp]
        public void SetUp()
        {
            ds.RemoveTable();
            ds.AddTable();
        }
        [TearDown]
        public void TearDown()
        {
            ds.RemoveTable();
        }

        static readonly SmartThingsDevice[] Devices = new SmartThingsDevice[]
        {
            new SmartThingsDevice("dfsfsfsda", "deadbeefae", "test"),
            new SmartThingsDevice("321123rasd", "asd", "testjhf"),
            new SmartThingsDevice("432sacgf34", "dasgh", "testjhf"),
            new SmartThingsDevice("43iodfkosdf", "dasd", "dsadg"),
        };
        static readonly SmartThingsDevice[] DuplicateDevices = new SmartThingsDevice[]
        {
            new SmartThingsDevice("43iodfkosdf", "dasd", "dsadg"),
            new SmartThingsDevice("43iodfkosdf", "dasd", "dsadg"),
        };

        public static IEnumerable<SmartThingsDevice> GetDevice()
        {
            foreach (var item in Devices)
            {
                yield return item;
            }
        }
        public async Task InsertAssertSuccess(SmartThingsDevice dev)
        {
            var success = await ds.SaveAsync(dev);
            Assert.IsTrue(success, "could not save device");
        }


        [Test, TestCaseSource("GetDevice")]
        public async Task Insert_NewDevice_ShouldSuccess(SmartThingsDevice dev)
        {
            await InsertAssertSuccess(dev);
        }

        public void AssertEqual(SmartThingsDevice original, SmartThingsDevice nonOriginal)
        {
            Assert.AreEqual(original.UUID, nonOriginal.UUID, "devices uuid are not equal");
            Assert.AreEqual(original.Name, nonOriginal.Name, "devices name are not equal");
            Assert.AreEqual(original.Key, nonOriginal.Key, "devices key are not equal");
        }

        public async Task InsertAssertEqual(SmartThingsDevice dev)
        {
            await InsertAssertSuccess(dev);
            var newDev = await ds.GetAsync(dev.UUID);
            Assert.NotNull(newDev, "could not retrieve back device");
            AssertEqual(newDev, newDev);
        }


        [Test, TestCaseSource("GetDevice")]
        public async Task InsertSelect_NewDevice_ShouldExist(SmartThingsDevice dev)
        {
            await InsertAssertEqual(dev);
        }

        [Test, TestCaseSource("GetDevice")]
        public async Task InsertUpdate_NewDevice_ShouldUpdate(SmartThingsDevice dev)
        {
            await InsertAssertEqual(dev);

            string newName = dev.Name + "🤡";

            var toUpdate = new SmartThingsDevice(dev.UUID, newName, dev.Key);

            var success = await ds.UpdateAsync(toUpdate);

            Assert.IsTrue(success, "could not update device");

            var newdev = await ds.GetAsync(dev.UUID);
            Assert.NotNull(newdev, "could not retrieve back updated device");
            AssertEqual(toUpdate, newdev);
        }
        [Test]
        public async Task InsertAll_AllShouldExist()
        {
            foreach (var dev in Devices)
            {
                await InsertAssertEqual(dev);
            }
        }
        // This test ensures that inserting devices with duplicate MAC addresses fails
        [Test]
        public void InsertAll_ShouldFail()
        {
            Assert.ThrowsAsync<SqliteException>(async () =>
            {
                foreach (var dev in DuplicateDevices)
                {
                    await ds.SaveAsync(dev);
                }
            });
        }
    }
}
