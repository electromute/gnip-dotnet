using System.Collections.Generic;

using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class PublisherTest
    {
        static RuleType actor = new RuleType("actor");
        static RuleType source = new RuleType("source");
        
        [Test]
        public void Properties() 
        {
            Publisher publisher = new Publisher("Digg", new RuleTypes() { actor, source });
            Assert.AreEqual("Digg", publisher.Name);
        }

        [Test]
        public void EqualitySemantics()
        {
            Assert.AreEqual(new Publisher("foo", new RuleTypes() { actor, source }), new Publisher("foo", new RuleTypes() { actor, source }));
        }

        [Test]
        public void CanSerialize()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><publisher xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""Digg""><supportedRuleTypes><type>actor</type><type>source</type></supportedRuleTypes></publisher>";

            Publisher publisher = new Publisher("Digg", new RuleTypes() { actor, source });
            Assert.AreEqual(xml, publisher.ToXml());
        }

        [Test]
        public void CanDeserialize()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><publisher xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""foo""><supportedRuleTypes><type>actor</type><type>source</type></supportedRuleTypes></publisher>";
            
            Publisher publisher = UTF8XmlSerializer.Deserialize<Publisher>(xml);
            Assert.AreEqual("foo", publisher.Name);
            Assert.IsTrue(publisher.RuleTypes.Count > 0);         
        }
    }
}
