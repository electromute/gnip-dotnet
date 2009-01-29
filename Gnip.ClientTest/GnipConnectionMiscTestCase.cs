using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Gnip.Client
{
    [TestFixture]
    public class GnipConnectionMiscTestCase : GnipTestCase
    {
        [TestFixtureSetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TestFixtureTearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestCalculateServerDelta()
        {
            base.gnipConnection.SyncToServerTime();
        }
    }
}
