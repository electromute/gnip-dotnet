using System.Collections.Generic;

using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class PublishersTest
    {
        [Test]
        public void CanSerialize()
        {
            Publishers publishers = new Publishers();
            publishers.Add(new Publisher("digg", new RuleTypes() { new RuleType("actor") }));

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?><publishers xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><publisher name=""digg""><supportedRuleTypes><type>actor</type></supportedRuleTypes></publisher></publishers>", publishers.ToXml());
        }

        [Test]
        public void CanDeserialize()
        {       
           Publishers publishers = UTF8XmlSerializer.Deserialize<Publishers>(@"<?xml version=""1.0"" encoding=""utf-8""?><publishers xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
<publisher name=""foo""><supportedRuleTypes><type>actor</type></supportedRuleTypes></publisher>
<publisher name=""bar""><supportedRuleTypes><type>actor</type></supportedRuleTypes></publisher>
</publishers>");
            Assert.AreEqual(2, publishers.Count);
            Assert.AreEqual("foo", publishers[0].Name);
            Assert.IsTrue(publishers[0].RuleTypes.Count == 1);
            Assert.AreEqual("actor", publishers[0].RuleTypes[0].Type);
            Assert.AreEqual("bar", publishers[1].Name);
            Assert.IsTrue(publishers[1].RuleTypes.Count == 1);
            Assert.AreEqual("actor", publishers[1].RuleTypes[0].Type);
        } 
    }
}
