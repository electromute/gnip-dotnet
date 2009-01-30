using System;
using Gnip.Client.Resource;
using System.Collections.Generic;
using NUnit.Framework;
using log4net;

namespace Gnip.Client
{
    [TestFixture]
    public class GnipConnectionFilterGzipTestCase : GnipConnectionFilterTestCase
    {
        private static ILog Log = LogManager.GetLogger(typeof(GnipConnectionFilterTestCase));

        [TestFixtureSetUp]
        public override void SetUp()
        {
            base.SetUp();
            config.UseGzip = true;
        }
    }
}