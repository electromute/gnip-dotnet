using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class RuleTest : BaseResourceTest
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
        public void TestRuleConstructor_01()
        {
            DateTime now = DateTime.Now;

            Rule rule = new Rule();
            rule.Type = RuleType.Actor;
            rule.Value = "foo";

            Rule rule2 = new Rule(RuleType.Actor,  "foo");

            Assert.AreEqual(rule.Type, RuleType.Actor);
            Assert.AreEqual(rule.Value, "foo");

            Assert.IsTrue(rule.DeepEquals(rule2));
        }

        [Test]
        public void TestRuleSerialize_01()
        {
            Rule rule = new Rule(RuleType.Actor, "foo");

            string str = XmlHelper.Instance.ToXmlString<Rule>(rule);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<rule xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" type=\"actor\">foo</rule>",
                str);
        }

        [Test]
        public void TestRuleDeserialize_01()
        {
            Rule rule = new Rule(RuleType.Actor, "foo");

            string str = XmlHelper.Instance.ToXmlString<Rule>(rule);
            Rule des = XmlHelper.Instance.FromXmlString<Rule>(str);
            Assert.IsTrue(rule.DeepEquals(des));
        }
    }
}
