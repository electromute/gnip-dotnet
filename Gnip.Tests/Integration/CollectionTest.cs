using NUnit.Framework;

namespace Gnip.Tests.Integration
{
    [TestFixture]
    public class CollectionTest
    {
        IConnection gnip;
        Collection collection;

        [SetUp]
        public void Setup()
        {
            collection = TestFactory.Collection();
            collection.Name = "apitestcollection" + TestFactory.TestRunGuid;

            gnip = TestFactory.LiveConnection();
            gnip.Create(collection);            
        }

        [TearDown]
        public void TearDown()
        {
            gnip.Delete(collection);
        }

        [Test]
        public void CanCreateCollection()
        {
            Assert.AreEqual(collection, gnip.GetCollection(collection.Name));
        }

        [Test]
        public void CanUpdateACollection()
        {
            collection.Uids.RemoveAt(0);
            collection.Uids.Add(new Uid(new Publisher("flickr"), "jeremy"));

            gnip.Update(collection);

            Assert.AreEqual(collection, gnip.GetCollection(collection.Name));
        }
    }
}
