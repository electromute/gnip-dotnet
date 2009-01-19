using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class RulesTest : BaseResourceTest
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
            Rules rules = new Rules();
            rules.Items.Add(new Rule(RuleType.Actor, "actor1"));
            rules.Items.Add(new Rule(RuleType.Regarding, "regarding1"));
            rules.Items.Add(new Rule(RuleType.Source, "source1"));

            Assert.AreEqual(3, rules.Items.Count);
        }

        [Test]
        public void TestPublisherSerialize_01()
        {
            Rules rules = new Rules();
            rules.Items.Add(new Rule(RuleType.Actor, "actor1"));
            rules.Items.Add(new Rule(RuleType.Regarding, "regarding1"));
            rules.Items.Add(new Rule(RuleType.Source, "source1"));

            string str = XmlHelper.Instance.ToXmlString<Rules>(rules);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><rules xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                                "<rule type=\"actor\">actor1</rule>" +
                                "<rule type=\"regarding\">regarding1</rule>" +
                                "<rule type=\"source\">source1</rule>" +
                            "</rules>", str);
        }

        [Test]
        public void TestPublisherDeserialize_01()
        {
            Rules rules = new Rules();
            rules.Items.Add(new Rule(RuleType.Actor, "actor1"));
            rules.Items.Add(new Rule(RuleType.Regarding, "regarding1"));
            rules.Items.Add(new Rule(RuleType.Source, "source1"));

            string str = XmlHelper.Instance.ToXmlString<Rules>(rules);
            Rules des = XmlHelper.Instance.FromXmlString<Rules>(str);

            Assert.AreEqual(3, des.Items.Count);
            for (int idx = 0; idx < des.Items.Count; idx++)
            {
                Assert.IsTrue(rules.Items[idx].DeepEquals(des.Items[idx]));
            }
        }
    }
}
