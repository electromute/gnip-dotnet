using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class UidTest
    {
        [Test]
        public void Properties()
        {
            Uid uid = new Uid(new Publisher("Digg"), "jeremy");
            Assert.AreEqual(new Publisher("Digg"), uid.Publisher);
            Assert.AreEqual("jeremy", uid.Name);
        }

        [Test]
        public void EqualitySemantics()
        {
            Assert.AreEqual(new Uid(new Publisher("Digg"), "jeremy"), new Uid(new Publisher("Digg"), "jeremy"));
        }

        [Test]
        public void CanDeserialize()
        {
            Uid uid = UTF8XmlSerializer.Deserialize<Uid>(@"<uid name=""me"" publisher.name=""digg""/>");
            
            Assert.AreEqual("me", uid.Name);
            Assert.AreEqual("digg", uid.Publisher.Name);
        }
    }
}
