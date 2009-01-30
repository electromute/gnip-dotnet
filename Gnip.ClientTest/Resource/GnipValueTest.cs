using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class GnipValueTest : BaseResourceTest
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
        public void TestGnipValueConstructor_01()
        {
            GnipValue gnipValue = new GnipValue();
            gnipValue.MetaUrl = "metaUrl 1";
            gnipValue.Value = "value 1";

            GnipValue gnipValue2 = new GnipValue(
                "value 1",
                "metaUrl 1");

            Assert.IsTrue(gnipValue.DeepEquals(gnipValue2));

        }

        private void TestDeepEquals(GnipValue objectA, GnipValue objectB, bool expect, bool expectDeep)
        {
            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestGnipValueConstructor_02()
        {
            this.TestDeepEquals(
                new GnipValue("value 1"),
                new GnipValue("value 1"),
                true, true);

            this.TestDeepEquals(
                new GnipValue("value 2"),
                new GnipValue("value 1"),
                false, false);
        }

        [Test]
        public void TestGnipValueConstructor_04()
        {
            this.TestDeepEquals(
                new GnipValue("value 1", "metaUrl 1"),
                new GnipValue("value 1", "metaUrl 1"),
                true, true);

            this.TestDeepEquals(
                new GnipValue("value 2", "metaUrl 1"),
                new GnipValue("value 1", "metaUrl 1"),
                false, false);

            this.TestDeepEquals(
                new GnipValue("value 1", "metaUrl 2"),
                new GnipValue("value 1", "metaUrl 1"),
                false, false);
        }

        [Test]
        public void TestGnipValueSerialize_01()
        {
            GnipValue gnipValue = new GnipValue(
                "value 1",
                "metaUrl 1");

            string str = XmlHelper.Instance.ToXmlString<GnipValue>(gnipValue);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<gnipValue xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" metaURL=\"metaUrl 1\">value 1</gnipValue>",
                            str);
        }

        [Test]
        public void TestGnipValueDeserialize_01()
        {
            GnipValue gnipValue = new GnipValue(
                "value 1",
                "metaUrl 1");

            string str = XmlHelper.Instance.ToXmlString<GnipValue>(gnipValue);

            GnipValue des = XmlHelper.Instance.FromXmlString<GnipValue>(str);
            Assert.IsTrue(gnipValue.DeepEquals(des));
        }
    }
}
