using Microsoft.Data.Sqlite;
using NUnit.Framework;
using xPowerHub.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPowerHub.Test.DataStore
{
    [TestFixture]
    internal class WizDSTest
    {

        private WizDS ds;
        public WizDSTest()
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
        static readonly WizDevice[] Devices = new WizDevice[]
        {
            new WizDevice("192.123.123.32", "deadbeefae", "test"),
            new WizDevice("192.123.123.d", "asd", "testjhf"),
            new WizDevice("192.123.123.d", "dasgh", "testjhf"),
            new WizDevice("192.43.123.d", "dasd", "dsadg"),
        };
        static readonly WizDevice[] DuplicateDevices = new WizDevice[]
        {
            new WizDevice("192.123.123.d", "dasgh", "testjhf"),
            new WizDevice("192.43.123.d", "dasgh", "dsadg"),
        };
        public static IEnumerable<WizDevice> GetDevice()
        {
            foreach (var item in Devices)
            {
                yield return item;
            }
        }

        public async Task InsertAssertSuccess(WizDevice dev)
        {
            var success = await ds.SaveAsync(dev);
            Assert.IsTrue(success, "could not save device error");
        }
        

        [Test, TestCaseSource("GetDevice")]
        public async Task Insert_NewDevice_ShouldSuccess(WizDevice dev)
        {
            await InsertAssertSuccess(dev);
        }

        public void AssertEqual(WizDevice original, WizDevice nonOriginal)
        {
            Assert.AreEqual(original.IP, nonOriginal.IP, "device ip is not equal to stored device");
            Assert.AreEqual(original.Name, nonOriginal.Name, "device name is not equal to stored device");
        }

        public async Task InsertAssertEqual(WizDevice dev)
        {
            await InsertAssertSuccess(dev);
            var storedDev = await ds.GetAsync(dev.MAC);
            Assert.NotNull(storedDev, "could not retrieve back device");
            AssertEqual(dev, storedDev);
        }


        [Test, TestCaseSource("GetDevice")]
        public async Task InsertSelect_NewDevice_ShouldExist(WizDevice dev)
        {
            await InsertAssertEqual(dev);
        }

        [Test, TestCaseSource("GetDevice")]
        public async Task InsertUpdate_NewDevice_ShouldUpdate(WizDevice dev)
        {
            await InsertAssertEqual(dev);

            string newIp = dev.IP + "🤡";

            string newName = dev.Name + "🤪";

            var toUpdate = new WizDevice(newIp, dev.MAC, newName);

            var success = await ds.UpdateAsync(toUpdate);

            Assert.IsTrue(success, "could not update device");

            var newdev = await ds.GetAsync(dev.MAC);
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
        public void InsertAll_ShouldThrow()
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
