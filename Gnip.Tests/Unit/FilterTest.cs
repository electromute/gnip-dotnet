using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class FilterTest
    {
        static Rule me = new Rule("actor", "me");
        static Rule you = new Rule("actor", "you");

        [Test]
        public void Properties()
        {
            Filter filter = new Filter("mine", "false", me, you);

            Assert.AreEqual("mine", filter.Name);
            Assert.AreEqual(me, filter.Rules[0]);
        }

        [Test]
        public void EqualitySemantics()
        {
            Assert.AreEqual(new Filter("mine", "false", me, you),
                            new Filter("mine", "false", me, you));
            Assert.AreNotEqual(new Filter("mine", "false", me, you),
                               new Filter("mine", "false", me));
        }

        [Test]
        public void CanSerialize()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = new Filter("mine", "false", me, you);
            Assert.AreEqual(xml, filter.ToXml());
        }

        [Test]
        public void CanSerializeWithJid()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><jid>me@example.com</jid><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = new Filter("mine", "false", me, you);
            filter.SetJid("me@example.com");
            Assert.AreEqual(xml, filter.ToXml());
        }

        [Test]
        public void CanSerializeWithPostUrl()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><postUrl>http://example.com</postUrl><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = new Filter("mine", "false", me, you);
            filter.SetPostUrl("http://example.com");
            Assert.AreEqual(xml, filter.ToXml());
        }

        [Test]
        public void CanDeserialize()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = UTF8XmlSerializer.Deserialize<Filter>(xml);
            Assert.AreEqual("mine", filter.Name);
        }

        [Test]
        public void CanDeserializeWithJid()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><jid>me@example.com</jid><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = UTF8XmlSerializer.Deserialize<Filter>(xml);
            Assert.AreEqual("me@example.com", filter.OptionalItem);
        }

        [Test]
        public void CanDeserializeWithPostUrl()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><postUrl>http://example.com</postUrl><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = UTF8XmlSerializer.Deserialize<Filter>(xml);
            Assert.AreEqual("http://example.com", filter.OptionalItem);
        }
    }
}
