using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class PublishersTest : BaseResourceTest
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
            Publishers publishers = new Publishers();
            publishers.Items.Add(new Publisher(PublisherType.My, "foobar1", RuleType.Actor, RuleType.Tag));
            publishers.Items.Add(new Publisher(PublisherType.My, "foobar2", RuleType.Actor, RuleType.Tag));
            publishers.Items.Add(new Publisher(PublisherType.My, "foobar3", RuleType.Actor, RuleType.Tag));

            Assert.AreEqual(3, publishers.Items.Count);
        }

        [Test]
        public void TestPublisherSerialize_01()
        {
            Publishers publishers = new Publishers();
            publishers.Items.Add(new Publisher(PublisherType.My, "foobar1", RuleType.Actor, RuleType.Tag));
            publishers.Items.Add(new Publisher(PublisherType.My, "foobar2", RuleType.Actor, RuleType.Tag));
            publishers.Items.Add(new Publisher(PublisherType.My, "foobar3", RuleType.Actor, RuleType.Tag));

            string str = XmlHelper.Instance.ToXmlString<Publishers>(publishers);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<publishers xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                                "<publisher name=\"foobar1\">" +
                                    "<supportedRuleTypes>" +
                                        "<type>actor</type>" +
                                        "<type>tag</type>" +
                                    "</supportedRuleTypes>" +
                                "</publisher>" +
                                "<publisher name=\"foobar2\">" +
                                    "<supportedRuleTypes>" +
                                        "<type>actor</type>" +
                                        "<type>tag</type>" +
                                    "</supportedRuleTypes>" +
                                "</publisher>" +
                                "<publisher name=\"foobar3\">" +
                                    "<supportedRuleTypes>" +
                                        "<type>actor</type>" +
                                        "<type>tag</type>" +
                                    "</supportedRuleTypes>" +
                                "</publisher>" +
                            "</publishers>",
                            str);
        }

        [Test]
        public void TestPublisherDeserialize_01()
        {
            Publishers publishers = new Publishers();
            publishers.Items.Add(new Publisher(PublisherType.Gnip, "foobar1", RuleType.Actor, RuleType.Tag));
            publishers.Items.Add(new Publisher(PublisherType.Gnip, "foobar2", RuleType.Actor, RuleType.Tag));
            publishers.Items.Add(new Publisher(PublisherType.Gnip, "foobar3", RuleType.Actor, RuleType.Tag));

            string str = XmlHelper.Instance.ToXmlString<Publishers>(publishers);
            Publishers des = XmlHelper.Instance.FromXmlString<Publishers>(str);
            Assert.AreEqual(3, des.Items.Count);

            for (int idx = 0; idx < des.Items.Count; idx++)
            {
                Assert.IsTrue(publishers.Items[idx].DeepEquals(des.Items[idx]));
            }
        }
    }
}
