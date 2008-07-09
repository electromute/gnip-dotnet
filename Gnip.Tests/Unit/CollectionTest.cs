using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class CollectionTest
    {
        static Publisher digg = new Publisher("digg");
        static Publisher flickr = new Publisher("flickr");
        static Uid me = new Uid(digg, "me");
        static Uid you = new Uid(flickr, "you");

        [Test]
        public void Properties()
        {
            Collection collection = new Collection("mine", me, you);

            Assert.AreEqual("mine", collection.Name);
            Assert.AreEqual(me, collection.Uids[0]);
        }

        [Test]
        public void EqualitySemantics()
        {
            Assert.AreEqual(new Collection("foo", me, you),
                            new Collection("foo", me, you));
            Assert.AreNotEqual(new Collection("foo", me, you),
                               new Collection("foo", me));
        }

        [Test]
        public void CanDeserialize()
        {
            string xml =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<collection xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" name=""example1"">
    <uid name=""john.doe"" publisher.name=""twitter"" />
    <uid name=""jane"" publisher.name=""digg"" />
</collection>";

            Collection collection = UTF8XmlSerializer.Deserialize<Collection>(xml);
            Assert.AreEqual(xml, collection.ToXml());
        }
    }
}
