using Microsoft.Data.Sqlite;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPowerHub.Test
{
    [TestFixture]
    internal class WizDSTest
    {

        private DataStore.WizDS ds;
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
        static readonly WizDevice[] WizDevs = new WizDevice[]
        {
            new WizDevice("192.123.123.32", "deadbeefae", "test"),
            new WizDevice("192.123.123.d", "asd", "testjhf"),
            new WizDevice("192.123.123.d", "dasgh", "testjhf"),
            new WizDevice("192.43.123.d", "dasd", "dsadg"),
        };
        static readonly WizDevice[] DuplicateEntries = new WizDevice[]
        {
            new WizDevice("192.123.123.d", "dasgh", "testjhf"),
            new WizDevice("192.43.123.d", "dasgh", "dsadg"),
        };
        public static IEnumerable<WizDevice> GetDevice()
        {
            foreach (var item in WizDevs)
            {
                yield return item;
            }
        }


        public async Task InsertAssertSuccess(WizDevice wiz)
        {
            var success = await ds.SaveAsync(wiz);
            Assert.IsTrue(success, "could not save device error");
        }
        

        [Test, TestCaseSource("GetDevice")]
        public async Task Insert_NewDevice_ShouldSuccess(WizDevice wiz)
        {
            await InsertAssertSuccess(wiz);
        }

        public void AssertEqual(WizDevice original, WizDevice nonOriginal)
        {
            Assert.AreEqual(original.IP, nonOriginal.IP, "device ip is not equal to stored device");
            Assert.AreEqual(original.Name, nonOriginal.Name, "device name is not equal to stored device");
        }

        public async Task InsertAssertEqual(WizDevice wiz)
        {
            await InsertAssertSuccess(wiz);
            var dev = await ds.GetAsync(wiz.MAC);
            Assert.NotNull(dev, "could not retrieve back device");
            AssertEqual(wiz, dev);
        }


        [Test, TestCaseSource("GetDevice")]
        public async Task InsertSelect_NewDevice_ShouldExist(WizDevice wiz)
        {
            await InsertAssertEqual(wiz);
        }

        [Test, TestCaseSource("GetDevice")]
        public async Task InsertUpdate_NewDevice_ShouldUpdate(WizDevice wiz)
        {
            await InsertAssertEqual(wiz);

            string newIp = wiz.IP + "🤡";

            string newName = wiz.Name + "🤪";

            var toUpdate = new WizDevice(newIp, wiz.MAC, newName);

            var success = await ds.UpdateAsync(toUpdate);

            Assert.IsTrue(success, "could not update device");

            var newdev = await ds.GetAsync(wiz.MAC);
            Assert.NotNull(newdev, "could not retrieve back updated device");
            AssertEqual(toUpdate, newdev);
        }
        [Test]
        public async Task InsertAll_AllShouldExist()
        {
            foreach (var dev in WizDevs)
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
                foreach (var dev in DuplicateEntries)
                {
                    await ds.SaveAsync(dev);
                }
            });
        }
    }
}
