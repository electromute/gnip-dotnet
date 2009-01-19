using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Gnip.Client
{
    [TestFixture]
    public class GnipConnectionTest
    {
        [Test]
        public void TestFloor()
        {
            DateTime date = new DateTime(2009, 1, 15, 1, 1, 0);

            Assert.AreNotEqual(date, GnipConnection.GetBucketFloor(new DateTime(2009, 1, 15, 1, 0, 59)));
            Assert.AreEqual(date, GnipConnection.GetBucketFloor(new DateTime(2009, 1, 15, 1, 1, 0)));
            Assert.AreEqual(date, GnipConnection.GetBucketFloor(new DateTime(2009, 1, 15, 1, 1, 10)));
            Assert.AreEqual(date, GnipConnection.GetBucketFloor(new DateTime(2009, 1, 15, 1, 1, 20)));
            Assert.AreEqual(date, GnipConnection.GetBucketFloor(new DateTime(2009, 1, 15, 1, 1, 30)));
            Assert.AreEqual(date, GnipConnection.GetBucketFloor(new DateTime(2009, 1, 15, 1, 1, 40)));
            Assert.AreEqual(date, GnipConnection.GetBucketFloor(new DateTime(2009, 1, 15, 1, 1, 50)));
            Assert.AreEqual(date, GnipConnection.GetBucketFloor(new DateTime(2009, 1, 15, 1, 1, 59)));
            Assert.AreNotEqual(date, GnipConnection.GetBucketFloor(new DateTime(2009, 1, 15, 1, 2, 00)));
        }
    }
}
