using NUnit.Framework;

namespace Gnip.Tests.Integration
{
    [TestFixture]
    public class PublisherTest
    {
        IConnection gnip;
        Publisher publisher;

        [SetUp]
        public void Setup()
        {
            publisher = TestFactory.ExistingPublisher;
            gnip = TestFactory.LiveConnection();
        }

        [Test]
        public void CanGetPublisher()
        {
            CollectionAssert.Contains(gnip.GetPublishers(), publisher);
            Assert.AreEqual(publisher, gnip.GetPublisher(publisher.Name));
        }

        [Test]
        public void CanPublishToPublisher()
        {
            Activities activities = TestFactory.Activities();

            gnip.Publish(publisher, activities);

            CollectionAssert.Contains(gnip.GetActivities(publisher), activities[0]);
        }
    }
}
