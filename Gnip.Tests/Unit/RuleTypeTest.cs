using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class RuleTypeTest
    {
        [Test]
        public void Properties()
        {
            RuleType ruleType = new RuleType("actor");
            Assert.AreEqual("actor", ruleType.Type);
        }

        [Test]
        public void EqualitySemantics()
        {
            Assert.AreEqual(new RuleType("actor"), new RuleType("actor"));
        }

        [Test]
        public void CanSerialize()
        {
            RuleType ruleType = new RuleType("actor");

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?><type xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">actor</type>", ruleType.ToXml());
        }

        [Test]
        public void CanDeserialize()
        {
            RuleType ruleType = UTF8XmlSerializer.Deserialize<RuleType>(@"<?xml version=""1.0"" encoding=""utf-8""?><type xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">actor</type>");

            Assert.AreEqual("actor", ruleType.Type);
        }
    }
}
