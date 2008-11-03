using System.Collections.Generic;

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
            Filter filter = new Filter("mine", "false", null, new List<Rule>(){me, you});

            Assert.AreEqual("mine", filter.Name);
            Assert.AreEqual(me, filter.Rules[0]);
        }

        [Test]
        public void EqualitySemantics()
        {
            Assert.AreEqual(new Filter("mine", "false", null, new List<Rule>() { me, you }),
                            new Filter("mine", "false", null, new List<Rule>() { me, you }));
            Assert.AreNotEqual(new Filter("mine", "false", null, new List<Rule>() { me, you }),
                               new Filter("mine", "false", null, new List<Rule>() { me }));
        }

        [Test]
        public void CanSerialize()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = new Filter("mine", "false", null, new List<Rule>() { me, you });
            Assert.AreEqual(xml, filter.ToXml());
        }

        [Test]
        public void CanSerializeWithPostUrl()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><postUrl>http://example.com</postUrl><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = new Filter("mine", "false", "http://example.com", new List<Rule>() { me, you });
            Assert.AreEqual(xml, filter.ToXml());
        }

        [Test]
        public void CanDeserialize()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = UTF8XmlSerializer.Deserialize<Filter>(xml);
            Assert.AreEqual("mine", filter.Name);
            Assert.IsTrue(filter.Rules.Count > 0);
        }

        [Test]
        public void CanDeserializeWithPostUrl()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""mine"" fullData=""false""><postUrl>http://example.com</postUrl><rule type=""actor"" value=""me"" /><rule type=""actor"" value=""you"" /></filter>";

            Filter filter = UTF8XmlSerializer.Deserialize<Filter>(xml);
            Assert.AreEqual("http://example.com", filter.PostUrl);
        }
    }
}
