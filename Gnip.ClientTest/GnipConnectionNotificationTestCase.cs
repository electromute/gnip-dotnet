using System;
using Gnip.Client.Resource;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Gnip.Client
{
    [TestFixture]
	public class GnipConnectionNotificationTestCase : GnipTestCase
	{
        [TestFixtureSetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TestFixtureTearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

		[Test]
		public void TestGetNotificationForPublisherFromGnip()
		{
			gnipConnection.Publish(localPublisher, this.activities);
			
			WaitForServerWorkToComplete();
			
			Activities activities = gnipConnection.GetNotifications(localPublisher);
            Assert.IsNotNull(activities);
            int idx = activities.Items.Count - 1;
            Assert.IsTrue(activities.Items.Count >= 2);
            Assert.AreEqual(activity1.Action, activities.Items[idx - 2].Action);
            Assert.AreEqual(activity2.Action, activities.Items[idx - 1].Action);
		}

        [Test]
		public void TestGetNotificationForPublisherFromGnipWithTime()
		{
			gnipConnection.Publish(localPublisher, this.activities);
			
			WaitForServerWorkToComplete();
			
			DateTime bucketTime = new DateTime();
			Activities activities = gnipConnection.GetNotifications(localPublisher, bucketTime);
            Assert.IsNotNull(activities);
            int idx = activities.Items.Count - 1;
            Assert.IsTrue(activities.Items.Count >= 2);
            Assert.AreEqual(activity1.Action, activities.Items[idx - 2].Action);
            Assert.AreEqual(activity2.Action, activities.Items[idx - 1].Action);
		}

        [Test]
		public void TestGetNotificationForFilterFromGnip()
		{
            Assert.IsFalse(notificationFilterToCreate.IsFullData);
			try
			{
				gnipConnection.Create(localPublisher, notificationFilterToCreate);
				
				WaitForServerWorkToComplete();

                Filter filter = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, notificationFilterToCreate.Name);
                Assert.IsNotNull(filter);
				
				gnipConnection.Publish(localPublisher, this.activities);
				
				WaitForServerWorkToComplete();
				
				Activities activities = gnipConnection.GetActivities(localPublisher, notificationFilterToCreate);
                Assert.IsNotNull(activities);
                Assert.IsTrue(activities.Items.Count > 0);
                Assert.AreEqual(activity3.Action, activities.Items[0].Action);
			}
			finally
			{
				gnipConnection.Delete(localPublisher, notificationFilterToCreate);
			}
		}

        [Test]
		public void TestGetNotificationForFilterFromGnipWithTime()
		{
            Assert.IsFalse(notificationFilterToCreate.IsFullData);
			try
			{
				gnipConnection.Create(localPublisher, notificationFilterToCreate);
				
				WaitForServerWorkToComplete();

                Filter filter = gnipConnection.GetFilter(localPublisher.Type, localPublisher.Name, notificationFilterToCreate.Name);
                Assert.IsNotNull(filter);
				
				gnipConnection.Publish(localPublisher, this.activities);
				
				WaitForServerWorkToComplete();
				
				Activities activities = gnipConnection.GetActivities(localPublisher, notificationFilterToCreate, new DateTime());
                Assert.IsNotNull(activities);
                Assert.IsTrue(activities.Items.Count > 0);
                int idx = activities.Items.Count;
                Assert.AreEqual(activity3.Action, activities.Items[idx - 1].Action);
			}
			finally
			{
				gnipConnection.Delete(localPublisher, notificationFilterToCreate);
			}
		}
	}
}