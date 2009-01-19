using System;
using Gnip.Client.Resource;
using System.Collections.Generic;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client
{
    [TestFixture]
    public class GnipConnectionPublisherTest : GnipTestCase
    {
        [TestFixtureSetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TestFixtureTearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestGetPublisher()
        {
            Publisher publisher = gnipConnection.GetPublisher(localPublisher.Type, localPublisher.Name);
            Assert.IsNotNull(publisher);
            Assert.AreEqual(localPublisher.Name, publisher.Name);
        }

        [Test]
        public void TestGetPublisherIncludesCapabilities()
        {
            Publisher publisher = gnipConnection.GetPublisher(localPublisher.Type, localPublisher.Name);
            Assert.IsNotNull(publisher);
            Assert.IsTrue(localPublisher.SupportedRuleTypes.Contains(RuleType.Actor));
        }

        [Test]
        public void TestUpdatePublisher()
        {
            Publisher publisher = gnipConnection.GetPublisher(localPublisher.Type, localPublisher.Name);
            publisher.SupportedRuleTypes = new List<RuleType>() {
                RuleType.Actor};
            Result result = gnipConnection.Update(publisher);
            Assert.IsTrue(result.IsSuccess);

            publisher = gnipConnection.GetPublisher(localPublisher.Type, localPublisher.Name);
            Assert.AreEqual(1, publisher.SupportedRuleTypes.Count);
            Assert.AreEqual(RuleType.Actor, publisher.SupportedRuleTypes[0]);

            publisher.SupportedRuleTypes = new List<RuleType>() {
                RuleType.Actor,
                RuleType.Regarding,
                RuleType.Source,
                RuleType.Tag,
                RuleType.To
            };
            result = gnipConnection.Update(publisher);
            Assert.IsTrue(result.IsSuccess);
            publisher = gnipConnection.GetPublisher(localPublisher.Type, localPublisher.Name);
            Assert.AreEqual(5, publisher.SupportedRuleTypes.Count);
        }

        [Test]
        public void TestGetPublishers()
        {
            Publishers publishers = gnipConnection.GetPublishers(TestConfig.Instance.PublisherType);
            Assert.IsNotNull(publishers);

            bool areEqual = false;

            foreach (Publisher pub in publishers.Items)
            {
                if (pub.Name == localPublisher.Name)
                {
                    areEqual = true;
                    foreach (RuleType ruleType in localPublisher.SupportedRuleTypes)
                    {
                        assertContains(ruleType, pub.SupportedRuleTypes);
                    }
                }
            }
            Assert.IsTrue(areEqual);
        }
    }
}