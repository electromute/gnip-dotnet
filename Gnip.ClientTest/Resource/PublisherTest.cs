using System;
using System.Text;
using NUnit.Framework;
using System.Collections.Generic;
using Gnip.Client.Resource;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class PublisherTest : BaseResourceTest
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
        public void TestPublisherConstructor_01()
        {
            List<RuleType> oneRuleType = new List<RuleType>();
            oneRuleType.Add(RuleType.Actor);
            oneRuleType.Add(RuleType.Regarding);

            Publisher publisher = new Publisher(PublisherType.My, "foobar", oneRuleType);
            Assert.AreEqual(PublisherType.My, publisher.Type);
            Assert.AreEqual("foobar", publisher.Name);
            Assert.AreEqual(oneRuleType.Count, publisher.SupportedRuleTypes.Count);
            Assert.IsTrue(publisher.SupportedRuleTypes.Contains(RuleType.Actor));
            Assert.IsTrue(publisher.SupportedRuleTypes.Contains(RuleType.Regarding));

            Publisher publisher2 = new Publisher();
            publisher2.Name = "foobar";
            publisher2.SupportedRuleTypes.AddRange(oneRuleType);
            Assert.IsTrue(publisher2.SupportedRuleTypes.Contains(RuleType.Actor));
            Assert.IsTrue(publisher2.SupportedRuleTypes.Contains(RuleType.Regarding));
        }

        [Test]
        public void TestPublisherConstructor_02()
        {
            Publisher publisher = new Publisher(PublisherType.My, "foobar", RuleType.Actor, RuleType.Tag);
            Assert.AreEqual(PublisherType.My, publisher.Type);
            Assert.AreEqual("foobar", publisher.Name);
            Assert.AreEqual(2, publisher.SupportedRuleTypes.Count);
            Assert.IsTrue(publisher.SupportedRuleTypes.Contains(RuleType.Actor));
            Assert.IsTrue(publisher.SupportedRuleTypes.Contains(RuleType.Tag));
        }

        private void TestDeepEquals(Publisher objectA, Publisher objectB, bool expect, bool expectDeep)
        {
            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestPublisherConstructor_03()
        {
            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 1"),
                new Publisher(PublisherType.My, "name 1"),
                false, true);

            this.TestDeepEquals(
                new Publisher(PublisherType.Gnip, "name 1"),
                new Publisher(PublisherType.My, "name 1"),
                false, false);

            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 2"),
                new Publisher(PublisherType.My, "name 1"),
                false, false);
        }

        [Test]
        public void TestPublisherConstructor_04()
        {
            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 1", RuleType.Actor),
                new Publisher(PublisherType.My, "name 1", RuleType.Actor),
                false, true);

            this.TestDeepEquals(
                new Publisher(PublisherType.Gnip, "name 1", RuleType.Actor),
                new Publisher(PublisherType.My, "name 1", RuleType.Actor),
                false, false);

            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 2", RuleType.Actor),
                new Publisher(PublisherType.My, "name 1", RuleType.Actor),
                false, false);

            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 1", RuleType.Regarding),
                new Publisher(PublisherType.My, "name 1", RuleType.Actor),
                false, false);
        }

        [Test]
        public void TestPublisherConstructor_05()
        {
            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 1", RuleType.Actor, RuleType.Regarding),
                new Publisher(PublisherType.My, "name 1", RuleType.Actor, RuleType.Regarding),
                false, true);

            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 1", RuleType.Actor, RuleType.Regarding),
                new Publisher(PublisherType.My, "name 1", RuleType.Regarding, RuleType.Actor),
                false, true);

            this.TestDeepEquals(
                new Publisher(PublisherType.Gnip, "name 1", RuleType.Actor, RuleType.Regarding),
                new Publisher(PublisherType.My, "name 1", RuleType.Actor, RuleType.Regarding),
                false, false);

            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 2", RuleType.Actor, RuleType.Regarding),
                new Publisher(PublisherType.My, "name 1", RuleType.Actor, RuleType.Regarding),
                false, false);

            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 1", RuleType.Regarding, RuleType.Regarding),
                new Publisher(PublisherType.My, "name 1", RuleType.Actor, RuleType.Regarding),
                false, false);

            this.TestDeepEquals(
                new Publisher(PublisherType.My, "name 1", RuleType.Actor, RuleType.Actor),
                new Publisher(PublisherType.My, "name 1", RuleType.Actor, RuleType.Regarding),
                false, false);
        }

        [Test]
        public void TestPublisherSerialize_01()
        {
            Publisher publisher = new Publisher(PublisherType.My, "foobar", RuleType.Actor, RuleType.Tag);
            string str = XmlHelper.Instance.ToXmlString<Publisher>(publisher);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<publisher xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"foobar\">" +
                                "<supportedRuleTypes>" +
                                    "<type>actor</type>" +
                                    "<type>tag</type>" +
                                "</supportedRuleTypes>" +
                            "</publisher>",
                            str);
        }

        [Test]
        public void TestPublisherSerialize_02()
        {
            Publisher publisher = new Publisher(PublisherType.My, "foobar", RuleType.Actor, RuleType.Tag, RuleType.Source);
            string str = XmlHelper.Instance.ToXmlString<Publisher>(publisher);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<publisher xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"foobar\">" +
                                "<supportedRuleTypes>" +
                                    "<type>actor</type>" +
                                    "<type>tag</type>" +
                                    "<type>source</type>" +
                                "</supportedRuleTypes>" +
                            "</publisher>",
                            str);
        }

        [Test]
        public void TestPublisherSerialize_03()
        {
            Publisher publisher = new Publisher(PublisherType.My, "foobar");
            string str = XmlHelper.Instance.ToXmlString<Publisher>(publisher);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<publisher xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"foobar\">" +
                            "<supportedRuleTypes />" +
                            "</publisher>",
                            str);
        }

        [Test]
        public void TestPublisherDeserialize_01()
        {
            Publisher publisher = new Publisher(PublisherType.My, "foobar", RuleType.Actor, RuleType.Tag);
            string str = XmlHelper.Instance.ToXmlString<Publisher>(publisher);
            Publisher des = XmlHelper.Instance.FromXmlString<Publisher>(str);
            Assert.AreEqual(publisher.Name, des.Name);
            Assert.AreEqual(publisher.SupportedRuleTypes.Count, des.SupportedRuleTypes.Count);
            for (int idx = 0; idx < publisher.SupportedRuleTypes.Count; idx++)
            {
                Assert.AreEqual(publisher.SupportedRuleTypes[idx], des.SupportedRuleTypes[idx]);
            }
        }

        [Test]
        public void TestPublisherDeserialize_02()
        {
            Publisher publisher = new Publisher(PublisherType.My, "foobar", RuleType.Actor, RuleType.Tag, RuleType.Source);
            string str = XmlHelper.Instance.ToXmlString<Publisher>(publisher);
            Publisher des = XmlHelper.Instance.FromXmlString<Publisher>(str);
            Assert.AreEqual(publisher.Name, des.Name);
            Assert.AreEqual(publisher.SupportedRuleTypes.Count, des.SupportedRuleTypes.Count);
            for (int idx = 0; idx < publisher.SupportedRuleTypes.Count; idx++)
            {
                Assert.AreEqual(publisher.SupportedRuleTypes[idx], des.SupportedRuleTypes[idx]);
            }
        }

        [Test]
        public void TestPublisherDeserialize_03()
        {
            Publisher publisher = new Publisher(PublisherType.My, "foobar", RuleType.Actor);
            string str = XmlHelper.Instance.ToXmlString<Publisher>(publisher);
            Publisher des = XmlHelper.Instance.FromXmlString<Publisher>(str);
            Assert.AreEqual(publisher.Name, des.Name);
            Assert.AreEqual(publisher.SupportedRuleTypes.Count, des.SupportedRuleTypes.Count);
            for (int idx = 0; idx < publisher.SupportedRuleTypes.Count; idx++)
            {
                Assert.AreEqual(publisher.SupportedRuleTypes[idx], des.SupportedRuleTypes[idx]);
            }
        }
    }
}
