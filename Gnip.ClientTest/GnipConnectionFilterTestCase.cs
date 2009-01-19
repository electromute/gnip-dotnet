using System;
using Gnip.Client.Resource;
using System.Collections.Generic;
using NUnit.Framework;
using log4net;

namespace Gnip.Client
{
    [TestFixture]
    public class GnipConnectionFilterTestCase : GnipTestCase
    {
        private static ILog Log = LogManager.GetLogger(typeof(GnipConnectionFilterTestCase));

        [TestFixtureSetUp]
        public override void SetUp()
        {
            base.SetUp();
            config.TunnelOverPost = false;
        }

        [TestFixtureTearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestGetFilter()
        {
            Filter existingFilter = new Filter("existingFilter");
            try
            {
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "joe"));
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "jane"));
                gnipConnection.Create(localPublisher, existingFilter);

                WaitForServerWorkToComplete();

                Filter existing = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, existingFilter.Name);

                WaitForServerWorkToComplete();

                Assert.IsNotNull(existing);
                Assert.AreEqual(2, existing.Rules.Count);
            }
            finally
            {
                gnipConnection.Delete(localPublisher, existingFilter);
            }
        }

        [Test]
        public void TestCreateFilter()
        {
            gnipConnection.Create(localPublisher, filterToCreate);

            WaitForServerWorkToComplete();

            try
            {
                Filter filter = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, filterToCreate.Name);
                Assert.IsNotNull(filter);
                Assert.AreEqual(filterToCreate.Name, filter.Name);
                Assert.AreEqual(1, filter.Rules.Count);
                Rule rule = filter.Rules[0];
                Assert.AreEqual(RuleType.Actor, rule.Type);
                Assert.AreEqual("tom", rule.Value);
            }
            finally
            {
                gnipConnection.Delete(localPublisher, filterToCreate);
            }
        }

        [Test]
        public void TestUpdateFilter()
        {
            try
            {
                gnipConnection.Create(localPublisher, filterToCreate);

                WaitForServerWorkToComplete();

                Filter filter = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, filterToCreate.Name);
                filter.Rules.Add(new Rule(RuleType.Actor, "jojo"));
                gnipConnection.Update(localPublisher, filter);

                WaitForServerWorkToComplete();

                Filter updated = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, filterToCreate.Name);
                List<Rule> rules = updated.Rules;
                Assert.AreEqual(2, rules.Count);

                int idx = ("jojo".Equals(rules[0].Value) ? 0 : 1);

                Assert.AreEqual(RuleType.Actor, rules[idx].Type);
                Assert.AreEqual("jojo", rules[idx].Value);
            }
            finally
            {
                gnipConnection.Delete(localPublisher, filterToCreate);
            }
        }

        [Test]
        public void TestDeleteFilter()
        {
            Filter filter = null;
            try
            {
                gnipConnection.Create(localPublisher, filterToCreate);

                WaitForServerWorkToComplete();

                filter = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, filterToCreate.Name);
                Assert.IsNotNull(filter);
            }
            finally
            {
                gnipConnection.Delete(localPublisher, filter);
            }

            WaitForServerWorkToComplete();

            try
            {
                gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, filterToCreate.Name);
                Assert.Fail();
            }
            catch (GnipException)
            {
                //expected
            }
        }

        [Test]
        public void TestNoSuchFilter()
        {
            try
            {
                gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, "nosuchfilter");
                Assert.IsFalse(true, "Should have received exception for missing filter");
            }
            catch (GnipException)
            {
                // expected
            }
        }

        [Test]
        public void TestAddRuleToFilter()
        {
            try
            {
                gnipConnection.Create(localPublisher, filterToCreate);

                WaitForServerWorkToComplete();

                Rule ruleToAdd = new Rule(RuleType.Actor, "jojo");
                gnipConnection.Update(localPublisher, filterToCreate, ruleToAdd);

                WaitForServerWorkToComplete();

                Filter updated = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, filterToCreate.Name);
                List<Rule> rules = updated.Rules;
                Assert.AreEqual(2, rules.Count);

                int idx = ("jojo".Equals(rules[0].Value) ? 0 : 1);

                Assert.AreEqual(RuleType.Actor, rules[idx].Type);
                Assert.AreEqual("jojo", rules[idx].Value);
            }
            finally
            {
                gnipConnection.Delete(localPublisher, filterToCreate);
            }
        }

        [Test]
        public void TestBatchAddRulesToFilter()
        {
            try
            {
                gnipConnection.Create(localPublisher, filterToCreate);

                WaitForServerWorkToComplete();

                Rule r1 = new Rule(RuleType.Actor, "jojo"), r2 = new Rule(RuleType.Actor, "moe"), r3 = new Rule(RuleType.Actor, "barney");

                Rules rules = new Rules();
                rules.Items.Add(r1);
                rules.Items.Add(r2);
                rules.Items.Add(r3);

                gnipConnection.Update(localPublisher, filterToCreate, rules);

                WaitForServerWorkToComplete();

                Filter updated = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, filterToCreate.Name);
                List<Rule> updatedRules = updated.Rules;
                Assert.AreEqual(4, updatedRules.Count);
                Assert.IsTrue(updatedRules.Contains(r1));
                Assert.IsTrue(updatedRules.Contains(r2));
                Assert.IsTrue(updatedRules.Contains(r3));
            }
            finally
            {
                gnipConnection.Delete(localPublisher, filterToCreate);
            }
        }

        [Test]
        public void TestDeleteRuleFromFilter()
        {
            try
            {
                gnipConnection.Create(localPublisher, filterToCreate);

                WaitForServerWorkToComplete();

                Rule ruleToDelete = new Rule(RuleType.Actor, "jojo");
                gnipConnection.Update(localPublisher, filterToCreate, ruleToDelete);

                WaitForServerWorkToComplete();

                Filter updated = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, filterToCreate.Name);
                List<Rule> rules = updated.Rules;
                Assert.AreEqual(2, rules.Count);

                int idx = ("jojo".Equals(rules[0].Value) ? 0 : 1);
                Assert.AreEqual(RuleType.Actor, rules[idx].Type);
                Assert.AreEqual("jojo", rules[idx].Value);

                gnipConnection.Delete(localPublisher, filterToCreate, ruleToDelete);

                WaitForServerWorkToComplete();

                updated = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, filterToCreate.Name);
                rules = updated.Rules;
                Assert.AreEqual(1, rules.Count);

                Assert.AreEqual(RuleType.Actor, rules[0].Type);
                Assert.AreEqual("tom", rules[0].Value);
            }
            finally
            {
                gnipConnection.Delete(localPublisher, filterToCreate);
            }
        }
    }
}