using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Mocks;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class ConnectionTest
    {
        DynamicMock server;
        IConnection gnip;
        Activities activitiesForTest;
        Filter filterForTest;
        Publisher digg = new Publisher("digg");

        [SetUp]
        public void Setup()
        {
            server = new DynamicMock(typeof(IServer));
            gnip = new Connection((IServer)server.MockInstance);

            activitiesForTest = TestFactory.Activities();
            filterForTest = TestFactory.Filter();
        }

        [TearDown]
        public void TearDown()
        {
            server.Verify();
        }

        // publishers

        [Test]
        public void CanListPublishers()
        {
            server.ExpectAndReturn("Get", "<publishers><publisher name=\"digg\"/></publishers>", "/publishers.xml");

            List<Publisher> publishers = gnip.GetPublishers();
            
            Assert.Contains(digg, publishers);
        }

        [Test]
        public void CanGetPublisher()
        {
            server.ExpectAndReturn("Get", digg.ToXml(), "/publishers/digg.xml");

            Assert.AreEqual(digg, gnip.GetPublisher("digg"));
        }

        [Test]
        public void CanCreatePublisher()
        {
            server.Expect("Post", "/publishers.xml", digg.ToXml());

            gnip.Create(digg);
        }


        // filters

        [Test]
        public void CanGetFilter()
        {
            server.ExpectAndReturn("Get", filterForTest.ToXml(), "/publishers/digg/filters/myfilter.xml");

            Assert.AreEqual(filterForTest, gnip.GetFilter("digg", "myfilter"));
        }

        [Test]
        public void CanCreateAFilter()
        {
            server.Expect("Post", "/publishers/digg/filters.xml", filterForTest.ToXml());

            gnip.Create("digg", filterForTest);
        }

        [Test]
        public void CanDeleteAFilter()
        {
            server.Expect("Delete", "/publishers/digg/filters/example1.xml");

            gnip.Delete("digg", "example1");
        }

        [Test]
        public void CanUpdateAFilter()
        {
            server.Expect("Put", "/publishers/digg/filters/example1.xml", filterForTest.ToXml());

            gnip.Update("digg", filterForTest);
        }

        // activities

        [Test]
        public void CanGetCurrentNotificationsPerPublisher()
        {
            server.ExpectAndReturn("Get", activitiesForTest.ToXml(), "/publishers/digg/notification/current.xml");
            
            CollectionAssert.AreEqual(activitiesForTest, gnip.GetPublisherNotifications("digg"));
        }

        [Test]
        public void CanGetCurrentActivitiesForFilter()
        {
            server.ExpectAndReturn("Get", activitiesForTest.ToXml(), "/publishers/digg/filters/myfilter/activity/current.xml");

            CollectionAssert.AreEqual(activitiesForTest, gnip.GetFilterActivities("digg", "myfilter"));
        }

        [Test]
        public void CanGetActivitiesForSpecificBucket()
        {
            server.ExpectAndReturn("Get", activitiesForTest.ToXml(), "/publishers/digg/activity/200807081758.xml");

            CollectionAssert.AreEqual(activitiesForTest, gnip.GetPublisherActivities("digg", DateTime.Parse("7/8/2008 5:58pm GMT")));
        }

        [Test]
        public void CanPostActivities()
        {
            server.Expect("Post", "/publishers/bob/activity.xml", activitiesForTest.ToXml());

            gnip.Publish("bob", activitiesForTest);
        }
    }
}
