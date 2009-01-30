using System;
using Gnip.Client.Resource;
using Gnip.Client.Utils;
using System.IO;
using System.IO.Compression;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

namespace Gnip.Client
{
    [TestFixture]
    public class GnipConnectionActivityGzipTestCase : GnipConnectionActivityTestCase
    {
        [TestFixtureSetUp]
        public override void SetUp()
        {
            base.SetUp();
            config.UseGzip = true;
        }
    }
}