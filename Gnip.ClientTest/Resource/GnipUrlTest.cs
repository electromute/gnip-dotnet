using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class GnipUrlTest : BaseResourceTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestGnipUrlConstructor_01()
        {
            GnipUrl gnipUrl = new GnipUrl();
            gnipUrl.MetaUrl = "metaUrl 1";
            gnipUrl.Url = "url 1";

            GnipUrl gnipUrl2 = new GnipUrl(
                "url 1",
                "metaUrl 1");

            Assert.IsTrue(gnipUrl.DeepEquals(gnipUrl2));

        }

        private void TestDeepEquals(GnipUrl objectA, GnipUrl objectB, bool expect, bool expectDeep)
        {
            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestGnipUrlConstructor_02()
        {
            this.TestDeepEquals(
                new GnipUrl("url 1"),
                new GnipUrl("url 1"),
                true, true);

            this.TestDeepEquals(
                new GnipUrl("url 2"),
                new GnipUrl("url 1"),
                false, false);
        }

        [Test]
        public void TestGnipUrlConstructor_04()
        {
            this.TestDeepEquals(
                new GnipUrl("url 1", "metaUrl 1"),
                new GnipUrl("url 1", "metaUrl 1"),
                true, true);

            this.TestDeepEquals(
                new GnipUrl("url 2", "metaUrl 1"),
                new GnipUrl("url 1", "metaUrl 1"),
                false, false);

            this.TestDeepEquals(
                new GnipUrl("url 1", "metaUrl 2"),
                new GnipUrl("url 1", "metaUrl 1"),
                false, false);
        }

        [Test]
        public void TestGnipUrlSerialize_01()
        {
            GnipUrl gnipUrl = new GnipUrl(
                "url 1",
                "metaUrl 1");

            string str = XmlHelper.Instance.ToXmlString<GnipUrl>(gnipUrl);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<gnipURL xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" metaURL=\"metaUrl 1\">url 1</gnipURL>",
                            str);
        }

        [Test]
        public void TestGnipUrlDeserialize_01()
        {
            GnipUrl gnipUrl = new GnipUrl(
                "url 1",
                "metaUrl 1");

            string str = XmlHelper.Instance.ToXmlString<GnipUrl>(gnipUrl);

            GnipUrl des = XmlHelper.Instance.FromXmlString<GnipUrl>(str);
            Assert.IsTrue(gnipUrl.DeepEquals(des));
        }
    }
}
