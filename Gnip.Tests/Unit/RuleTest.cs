using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class RuleTest
    {
        [Test]
        public void Properties()
        {
            Rule rule = new Rule("actor", "sally");
            Assert.AreEqual("actor", rule.Type);
            Assert.AreEqual("sally", rule.Value);
        }

        [Test]
        public void EqualitySemantics()
        {
            Assert.AreEqual(new Rule("actor", "sally"), new Rule("actor", "sally"));
        }

        [Test]
        public void CanSerialize()
        {
            Rule rule = new Rule("actor", "sally");

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?><rule xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""actor"" value=""sally"" />", rule.ToXml());
        }

        [Test]
        public void CanDeserialize()
        {
            Rule rule = UTF8XmlSerializer.Deserialize<Rule>(@"<?xml version=""1.0"" encoding=""utf-8""?><rule xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""actor"" value=""sally"" />");
            
            Assert.AreEqual("actor", rule.Type);
            Assert.AreEqual("sally", rule.Value);
        }
    }
}
