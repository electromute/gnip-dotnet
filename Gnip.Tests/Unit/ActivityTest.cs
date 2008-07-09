using System;
using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class ActivityTest
    {
        [Test]
        public void CanSerialize()
        {
            Activity a = new Activity();
            a.At = DateTime.Parse("2008-07-01T19:19:36-04:00").ToUniversalTime();
            a.Guid = "152623406";
            a.Type = "dugg";
            a.Uid = "joe";

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?>
<activity xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" uid=""joe"" at=""2008-07-01T23:19:36Z"" guid=""152623406"" type=""dugg"" />",
                            a.ToXml());

            Activities activities = new Activities();
            activities.Add(a);

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?>
<activities xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <activity uid=""joe"" at=""2008-07-01T23:19:36Z"" guid=""152623406"" type=""dugg"" />
</activities>",
            activities.ToXml());
        }

        [Test]
        public void CanDeserialize()
        {
            Activities activities = UTF8XmlSerializer.Deserialize<Activities>("<activities>" +
                "<activity at='2008-07-01T19:19:36-04:00' guid='152623406' type='dugg' uid='ATFlorio'/>" +
                "<activity at='2008-07-01T19:19:36-04:00' guid='152623406' type='foo' uid='ATFlorio'/>" +
                "</activities>");

            Assert.AreEqual(2, activities.Count);
            Assert.AreEqual("foo", activities[1].Type);
        }

        [Test]
        public void EqualitySemantics()
        {
            DateTime now = DateTime.Now;

            Assert.AreEqual(new Activity("me", "post", now, "http://foo.bar", new Publisher("foo")),
                            new Activity("me", "post", now, "http://foo.bar", new Publisher("foo")));
            Assert.AreEqual(new Activity("me", "post", now, "http://foo.bar"),
                            new Activity("me", "post", now, "http://foo.bar"));
            Assert.AreNotEqual(new Activity("me", "post", now, "http://foo.bar"),
                               new Activity("me", "post", now, "http://food.bar"));
            Assert.AreNotEqual(new Activity("me", "post", now, "http://foo.bar"),
                               new Activity("me", "post", now, "http://foo.bar", new Publisher("foo")));
        }
    }
}
