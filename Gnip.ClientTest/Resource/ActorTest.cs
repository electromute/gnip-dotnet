using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class ActorTest : BaseResourceTest
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
        public void TestActorConstructor_01()
        {
            DateTime now = DateTime.Now;

            Actor actor = new Actor();
            actor.MetaUrl = "metaUrl 1";
            actor.Uid = "uid 1";
            actor.Value = "value 1";

            Actor actor2 = new Actor(
                "value 1",
                "uid 1",
                "metaUrl 1");

            Assert.IsTrue(actor.DeepEquals(actor2));

        }

        private void TestDeepEquals(Actor objectA, Actor objectB, bool expect, bool expectDeep)
        {
            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestActorConstructor_02()
        {
            this.TestDeepEquals(
                new Actor("value 1"),
                new Actor("value 1"),
                true, true);

            this.TestDeepEquals(
                new Actor("value 2"),
                new Actor("value 1"),
                false, false);
        }

        [Test]
        public void TestActorConstructor_03()
        {
            this.TestDeepEquals(
                new Actor("value 1", "uid 1"),
                new Actor("value 1", "uid 1"),
                true, true);

            this.TestDeepEquals(
                new Actor("value 2", "uid 1"),
                new Actor("value 1", "uid 1"),
                false, false);

            this.TestDeepEquals(
                new Actor("value 1", "uid 2"),
                new Actor("value 1", "uid 1"),
                false, false);
        }

        [Test]
        public void TestActorConstructor_04()
        {
            this.TestDeepEquals(
                new Actor("value 1", "uid 1", "metaUrl 1"),
                new Actor("value 1", "uid 1", "metaUrl 1"),
                true, true);

            this.TestDeepEquals(
                new Actor("value 2", "uid 1", "metaUrl 1"),
                new Actor("value 1", "uid 1", "metaUrl 1"),
                false, false);

            this.TestDeepEquals(
                new Actor("value 1", "uid 2", "metaUrl 1"),
                new Actor("value 1", "uid 1", "metaUrl 1"),
                false, false);

            this.TestDeepEquals(
                new Actor("value 1", "uid 1", "metaUrl 2"),
                new Actor("value 1", "uid 1", "metaUrl 1"),
                false, false);
        }

        [Test]
        public void TestActorSerialize_01()
        {
            Actor actor = new Actor(
                "value 1",
                "uid 1",
                "metaUrl 1");

            string str = XmlHelper.Instance.ToXmlString<Actor>(actor);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<actor xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" metaURL=\"metaUrl 1\" uid=\"uid 1\">value 1</actor>",
                            str);
        }

        [Test]
        public void TestActorDeserialize_01()
        {
            Actor actor = new Actor(
                "value 1",
                "uid 1",
                "metaUrl 1");

            string str = XmlHelper.Instance.ToXmlString<Actor>(actor);

            Actor des = XmlHelper.Instance.FromXmlString<Actor>(str);
            Assert.IsTrue(actor.DeepEquals(des));
        }
    }
}
