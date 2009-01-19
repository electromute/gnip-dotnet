using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class FilterTest : BaseResourceTest
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
        public void TestFilterConstructor_01()
        {
            DateTime now = DateTime.Now;

            Filter filter = new Filter();
            filter.IsFullData = true;
            filter.PostUrl = "PostUrl";
            filter.Name = "FilterName";
            filter.Rules.Add(new Rule(RuleType.Actor, "ActorOne"));

            Filter filter2 = new Filter(
                "FilterName",
                "PostUrl",
                true,
                new Rule(RuleType.Actor, "ActorOne"));

            Assert.IsTrue(filter.DeepEquals(filter2));

        }

        private void TestDeepEquals(Filter objectA, Filter objectB, bool expect, bool expectDeep)
        {
            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestFilterConstructor_02()
        {
            this.TestDeepEquals(
                new Filter("name 1"),
                new Filter("name 1"),
                false, true);

            this.TestDeepEquals(
                new Filter("name 2"),
                new Filter("name 1"),
                false, false);
        }

        [Test]
        public void TestFilterConstructor_03()
        {
            this.TestDeepEquals(
                new Filter("name 1", "post url 1"),
                new Filter("name 1", "post url 1"),
                false, true);

            this.TestDeepEquals(
                new Filter("name 2", "post url 1"),
                new Filter("name 1", "post url 1"),
                false, false);

            this.TestDeepEquals(
                new Filter("name 1", "post url 2"),
                new Filter("name 1", "post url 1"),
                false, false);
        }

        [Test]
        public void TestFilterConstructor_04()
        {
            this.TestDeepEquals(
                new Filter("name 1", "post url 1", true),
                new Filter("name 1", "post url 1", true),
                false, true);

            this.TestDeepEquals(
                new Filter("name 2", "post url 1", true),
                new Filter("name 1", "post url 1", true),
                false, false);

            this.TestDeepEquals(
                new Filter("name 1", "post url 2", true),
                new Filter("name 1", "post url 1", true),
                false, false);

            this.TestDeepEquals(
                new Filter("name 1", "post url 1", false),
                new Filter("name 1", "post url 1", true),
                false, false);
        }

        [Test]
        public void TestFilterConstructor_05()
        {
            this.TestDeepEquals(
                new Filter("name 1", "post url 1", true, new Rule(RuleType.Actor, "value 1")),
                new Filter("name 1", "post url 1", true, new Rule(RuleType.Actor, "value 1")),
                false, true);

            this.TestDeepEquals(
                new Filter("name 2", "post url 1", true, new Rule(RuleType.Actor, "value 1")),
                new Filter("name 1", "post url 1", true, new Rule(RuleType.Actor, "value 1")),
                false, false);

            this.TestDeepEquals(
                new Filter("name 1", "post url 2", true, new Rule(RuleType.Actor, "value 1")),
                new Filter("name 1", "post url 1", true, new Rule(RuleType.Actor, "value 1")),
                false, false);

            this.TestDeepEquals(
                new Filter("name 1", "post url 1", false, new Rule(RuleType.Actor, "value 1")),
                new Filter("name 1", "post url 1", true, new Rule(RuleType.Actor, "value 1")),
                false, false);

            this.TestDeepEquals(
                new Filter("name 1", "post url 1", true, new Rule(RuleType.Regarding, "value 1")),
                new Filter("name 1", "post url 1", true, new Rule(RuleType.Actor, "value 1")),
                false, false);

            this.TestDeepEquals(
                new Filter("name 1", "post url 1", true, new Rule(RuleType.Actor, "value 2")),
                new Filter("name 1", "post url 1", true, new Rule(RuleType.Actor, "value 1")),
                false, false);
        }


        [Test]
        public void TestFilterSerialize_01()
        {
            Filter filter = new Filter(
                "FilterName",
                "PostUrl",
                true,
                new Rule(RuleType.Actor, "ActorOne"));

            string str = XmlHelper.Instance.ToXmlString<Filter>(filter);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<filter xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"FilterName\" fullData=\"true\">" +
                "<postURL>PostUrl</postURL>" +
                "<rule type=\"actor\">ActorOne</rule>" +
                "</filter>", str);
        }

        [Test]
        public void TestFilterSerialize_02()
        {
            Filter filter = new Filter(
                "FilterName",
                "PostUrl",
                true,
                new Rule(RuleType.Actor, "ActorOne"),
                new Rule(RuleType.Actor, "ActorTwo"));

            string str = XmlHelper.Instance.ToXmlString<Filter>(filter);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<filter xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"FilterName\" fullData=\"true\">" +
                "<postURL>PostUrl</postURL>" +
                "<rule type=\"actor\">ActorOne</rule>" +
                "<rule type=\"actor\">ActorTwo</rule>" +
                "</filter>", str);
        }

        [Test]
        public void TestFilterDeserialize_01()
        {
            Filter filter = new Filter(
                "FilterName",
                "PostUrl",
                true,
                new Rule(RuleType.Actor, "ActorOne"));

            string str = XmlHelper.Instance.ToXmlString<Filter>(filter);
            Filter des = XmlHelper.Instance.FromXmlString<Filter>(str);
            Assert.IsTrue(filter.DeepEquals(des));
        }
    }
}
