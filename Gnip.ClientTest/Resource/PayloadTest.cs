using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class PayloadTest : BaseResourceTest
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
        public void TestPayloadConstructor_01()
        {
            Payload payload = new Payload("title 1", "payload 1", "raw 1", false);
            Assert.AreEqual(payload.Title, "title 1");
            Assert.AreEqual(payload.Body, "payload 1");
            Assert.AreEqual(payload.DecodedRaw, "raw 1");

            Payload payload2 = new Payload("title 1", "payload 1", "raw 1", false);

            Assert.IsTrue(payload.DeepEquals(payload2));
        }

        private void TestDeepEquals(Payload objectA, Payload objectB, bool expect, bool expectDeep)
        {
            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestPayloadConstructor_02()
        {
            this.TestDeepEquals(
                new Payload("title 1", "payload 1", "raw 1", false),
                new Payload("title 1", "payload 1", "raw 1", false),
                false, true);

            this.TestDeepEquals(
                new Payload("title 2", "payload 1", "raw 1", false),
                new Payload("title 1", "payload 1", "raw 1", false),
                false, false);

            this.TestDeepEquals(
                new Payload("title 1", "payload 2", "raw 1", false),
                new Payload("title 1", "payload 1", "raw 1", false),
                false, false);

            this.TestDeepEquals(
                new Payload("title 1", "payload 1", "raw 2", false),
                new Payload("title 1", "payload 1", "raw 1", false),
                false, false);
        }

        [Test]
        public void TestPayloadSerialize_01()
        {
            Payload payload = new Payload();

            string str = XmlHelper.Instance.ToXmlString<Payload>(payload);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<payload xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" />",
                str);
        }

        [Test]
        public void TestPayloadSerialize_02()
        {
            Payload payload = new Payload("title 1", "payload 1", "raw 1", false);

            string str = XmlHelper.Instance.ToXmlString<Payload>(payload);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<payload xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                "<title>title 1</title>" + 
                "<body>payload 1</body>" +
                "<raw>H4sIAAAAAAAEAO29B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/IursKt39fwCZjk5TBQAAAA==</raw>" +
                "</payload>",
                str);
        }

        [Test]
        public void TestPayloadSerialize_03()
        {
            Payload payload = new Payload("title 1", "payload 1", new List<GnipUrl> { new GnipUrl("value 1") }, "raw 1", false);

            string str = XmlHelper.Instance.ToXmlString<Payload>(payload);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<payload xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                "<title>title 1</title>" +
                "<body>payload 1</body>" +
                "<mediaURL>value 1</mediaURL>" + 
                "<raw>H4sIAAAAAAAEAO29B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/IursKt39fwCZjk5TBQAAAA==</raw>" +
                "</payload>",
                str);
        }

        [Test]
        public void TestPayloadDeserialize_01()
        {
            Payload payload = new Payload("title 1", "payload 1", "raw 1", false);

            string str = XmlHelper.Instance.ToXmlString<Payload>(payload);
            Payload des = XmlHelper.Instance.FromXmlString<Payload>(str);
            Assert.IsTrue(payload.DeepEquals(des));
        }
    }
}
