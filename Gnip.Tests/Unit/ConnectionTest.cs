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
        Collection collectionForTest;
        Publisher digg = new Publisher("digg");

        [SetUp]
        public void Setup()
        {
            server = new DynamicMock(typeof(IServer));
            gnip = new Connection((IServer)server.MockInstance);

            activitiesForTest = TestFactory.Activities();
            collectionForTest = TestFactory.Collection();
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


        // collections

        [Test]
        public void CanGetCollection()
        {
            server.ExpectAndReturn("Get", collectionForTest.ToXml(), "/collections/mycollection.xml");

            Assert.AreEqual(collectionForTest, gnip.GetCollection("mycollection"));
        }

        [Test]
        public void CanCreateACollection()
        {
            server.Expect("Post", "/collections.xml", collectionForTest.ToXml());

            gnip.Create(collectionForTest);
        }

        [Test]
        public void CanDeleteACollection()
        {
            server.Expect("Delete", "/collections/bob.xml");

            gnip.Delete(new Collection("bob"));
        }

        [Test]
        public void CanUpdateACollection()
        {
            server.Expect("Put", "/collections/example1.xml", collectionForTest.ToXml());

            gnip.Update(collectionForTest);
        }

        // activities

        [Test]
        public void CanGetCurrentActivitiesPerPublisher()
        {
            server.ExpectAndReturn("Get", activitiesForTest.ToXml(), "/publishers/digg/activity/current.xml");
            
            CollectionAssert.AreEqual(activitiesForTest, gnip.GetActivities(new Publisher("digg")));
        }

        [Test]
        public void CanGetCurrentActivitiesForCollection()
        {
            server.ExpectAndReturn("Get", activitiesForTest.ToXml(), "/collections/mycollection/activity/current.xml");

            CollectionAssert.AreEqual(activitiesForTest, gnip.GetActivities(new Collection("mycollection")));
        }

        [Test]
        public void CanGetActivitiesForSpecificBucket()
        {
            server.ExpectAndReturn("Get", activitiesForTest.ToXml(), "/publishers/digg/activity/200807081755.xml");

            CollectionAssert.AreEqual(activitiesForTest, gnip.GetActivities(digg, DateTime.Parse("7/8/2008 5:58pm GMT")));
        }

        [Test]
        public void CanPostActivities()
        {
            server.Expect("Post", "/publishers/bob/activity.xml", activitiesForTest.ToXml());

            gnip.Publish(new Publisher("bob"), activitiesForTest);
        }
    }
}
