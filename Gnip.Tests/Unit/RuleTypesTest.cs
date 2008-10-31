using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class RuleTypesTest
    {
        [Test]
        public void CanSerialize()
        {
            RuleTypes ruleTypes = new RuleTypes() { new RuleType("actor") };

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?><supportedRuleTypes xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><type>actor</type></supportedRuleTypes>", ruleTypes.ToXml());
        }

        [Test]
        public void CanDeserialize()
        {
            RuleTypes ruleTypes = UTF8XmlSerializer.Deserialize<RuleTypes>(@"<?xml version=""1.0"" encoding=""utf-8""?><supportedRuleTypes xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
<type>actor</type>
<type>source</type>
</supportedRuleTypes>");
            Assert.AreEqual(2, ruleTypes.Count);
            Assert.AreEqual("actor", ruleTypes[0].Type);
            Assert.AreEqual("source", ruleTypes[1].Type);
        } 
    }
}
