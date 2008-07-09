using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class PublisherTest
    {
        [Test]
        public void Properties() 
        {
            Publisher publisher = new Publisher("Digg");
            Assert.AreEqual("Digg", publisher.Name);
        }

        [Test]
        public void EqualitySemantics()
        {
            Assert.AreEqual(new Publisher("foo"), new Publisher("foo"));
        }

        [Test]
        public void CanDeserialize()
        {
            Publishers publishers = UTF8XmlSerializer.Deserialize<Publishers>(@"<publishers>
<publisher name=""foo""/>
<publisher name=""bar""/>
</publishers>");
            Assert.AreEqual(2, publishers.Count);
            Assert.AreEqual("foo", publishers[0].Name);
            Assert.AreEqual("bar", publishers[1].Name);
        }
    }
}
