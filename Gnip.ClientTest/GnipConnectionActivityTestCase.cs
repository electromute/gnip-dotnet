using System;
using Gnip.Client.Resource;
using Gnip.Client.Utils;
using System.IO;
using System.IO.Compression;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

namespace Gnip.Client
{
    [TestFixture]
	public class GnipConnectionActivityTestCase : GnipTestCase
	{
        Random rand = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));

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
		public void TestPayloadEncodeDecode()
		{
            string title = "title";
			string body = "foo";
			string decodedRaw = "bar";
			string raw = EncodePayload(decodedRaw);
			
			// Payload created with an encoded "raw"
			Payload payload = new Payload(title, body, decodedRaw);
			Activity activity = CreateActivityWithPayload(payload);
			
			Activity decoded = XmlHelper.Instance.FromXmlString<Activity>(XmlHelper.Instance.ToXmlString<Activity>(activity));
			Assert.AreEqual(body, payload.Body);
			Assert.AreEqual(raw, payload.Raw);
			Assert.AreEqual(decodedRaw, payload.DecodedRaw);
			Assert.AreEqual(decoded.Payload.Body, payload.Body);
			Assert.AreEqual(decoded.Payload.Raw, payload.Raw);
			Assert.AreEqual(decoded.Payload.DecodedRaw, payload.DecodedRaw);
			
			// Payload created with an un-encoded "raw"
            payload = new Payload(title, body, decodedRaw, false);
			activity = CreateActivityWithPayload(payload);

            decoded = XmlHelper.Instance.FromXmlString<Activity>(XmlHelper.Instance.ToXmlString<Activity>(activity));
			Assert.AreEqual(body, payload.Body);
			Assert.AreEqual(raw, payload.Raw);
			Assert.AreEqual(decodedRaw, payload.DecodedRaw);
			Assert.AreEqual(decoded.Payload.Body, payload.Body);
			Assert.AreEqual(decoded.Payload.Raw, payload.Raw);
			Assert.AreEqual(decoded.Payload.DecodedRaw, payload.DecodedRaw);

            payload = new Payload(title, body, raw, true);
			activity = CreateActivityWithPayload(payload);

            decoded = XmlHelper.Instance.FromXmlString<Activity>(XmlHelper.Instance.ToXmlString<Activity>(activity));
			Assert.AreEqual(body, payload.Body);
			Assert.AreEqual(raw, payload.Raw);
			Assert.AreEqual(decodedRaw, payload.DecodedRaw);
			Assert.AreEqual(decoded.Payload.Body, payload.Body);
			Assert.AreEqual(decoded.Payload.Raw, payload.Raw);
			Assert.AreEqual(decoded.Payload.DecodedRaw, payload.DecodedRaw);
		}

        [Test]
		public void TestPublishActivityToGnip()
		{
			gnipConnection.Publish(localPublisher, activities);
		}

        [Test]
		public void TestPublishActivityWithPayloadToGnip()
		{
			Activities activities = new Activities();
			
			string payload = "joe's update payload";
			
			MemoryStream baos = new MemoryStream();
            GZipStream gos = new GZipStream(baos, CompressionMode.Compress);
			byte[] bytes = StringUtils.ToByteArray(payload);
			gos.Write(bytes, 0, bytes.Length);
			gos.Flush();
			bytes = StringUtils.ToByteArray(Convert.ToBase64String(baos.ToArray()));
			
			Activity activity = new Activity(new Actor("joe"), "update", new Payload("title", payload, StringUtils.ToString(bytes)));
            activities.Items.Add(activity);
			
			gnipConnection.Publish(localPublisher, activities);
		}
		
		
		// this test can only be run if your user has permission
		// to access full data from Gnip
        public void TestGetActivityWithPayloadForPublisherFromGnip_01()
        {
            Activities activities = new Activities();

            String body = "joe's update payload body";
            String raw = "joe's update body raw";
            Activity activity = new Activity(new Actor("joe"), "update", new Payload("title", body, raw, false));
            activities.Items.Add(activity);
            String encodedRaw = activity.Payload.Raw;

            gnipConnection.Publish(localPublisher, activities);

            WaitForServerWorkToComplete();

            activities = gnipConnection.GetActivities(localPublisher);
            Assert.IsNotNull(activities);
            List<Activity> activitiesList = activities.Items;
            int idx = activitiesList.Count - 1;
            Assert.AreEqual(activity.Action, activitiesList[idx].Action);
            Assert.AreEqual(body, activitiesList[idx].Payload.Body);
            Assert.AreEqual(encodedRaw, activitiesList[idx].Payload.Raw);
            Assert.AreEqual(raw, activitiesList[idx].Payload.DecodedRaw);
        }

        // this test can only be run if your user has permission
        // to access full data from Gnip
        public void TestGetActivityWithPayloadForPublisherFromGnip_02()
        {
            Activities activities = new Activities();

            String body = "joe's update payload body";
            String raw = "joe's update body raw";
            List<MediaUrl> mediaUrls = new List<MediaUrl>() { new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1") };
            Payload payload = new Payload("title", body, mediaUrls, raw, false);
            Activity activity = new Activity(new Actor("joe"), "update", payload);
            activities.Items.Add(activity);
            String encodedRaw = activity.Payload.Raw;

            gnipConnection.Publish(localPublisher, activities);

            WaitForServerWorkToComplete();

            activities = gnipConnection.GetActivities(localPublisher);
            Assert.IsNotNull(activities);
            List<Activity> activitiesList = activities.Items;
            int idx = activitiesList.Count - 1;
            Assert.AreEqual(activity.Action, activitiesList[idx].Action);
            Assert.IsTrue(payload.DeepEquals(activitiesList[idx].Payload));
        }

        [Test]
        public void TestGetActivityFromGnip()
        {
            gnipConnection.Publish(base.localPublisher, this.activities);

            WaitForServerWorkToComplete();

            Activities activities = gnipConnection.GetActivities(localPublisher);
            Assert.IsNotNull(activities);
            //Assert.AreEqual(3, activities.Items.Count);
            Assert.AreEqual(activity1.Action, activities.Items[0].Action);
            Assert.AreEqual(activity2.Action, activities.Items[1].Action);
            Assert.AreEqual(activity3.Action, activities.Items[2].Action);
        }

        [Test]
        public void TestGetActivityForFilterFromGnip_01()
        {
            Filter existingFilter = new Filter("existingFilter" + rand.Next());
            try
            {
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "joe"));
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "jane"));
                gnipConnection.Create(localPublisher, existingFilter);

                WaitForServerWorkToComplete();

                gnipConnection.Publish(base.localPublisher, this.activities);

                WaitForServerWorkToComplete();

                Activities activities = gnipConnection.GetActivities(localPublisher, existingFilter);
                Assert.IsNotNull(activities);
                Assert.AreEqual(activity1.Action, activities.Items[0].Action);
            }
            finally
            {
                Result res = gnipConnection.Delete(localPublisher, existingFilter);
                Assert.AreEqual(true, res.IsSuccess);
            }
        }

        [Test]
        public void TestGetActivityForFilterFromGnip_02()
        {
            Filter existingFilter = new Filter("existingFilter" + rand.Next(), null, true);
            try
            {
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "joe"));
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "jane"));
                gnipConnection.Create(localPublisher, existingFilter);

                WaitForServerWorkToComplete();

                Activities activities = new Activities();
                Activity activity1 = this.GetActivity1(1, "joe", "update1");
                activities.Items.Add(activity1);
                Activity activity2 = this.GetActivity1(2, "tom", "update2");
                activities.Items.Add(activity2);
                Activity activity3 = this.GetActivity1(3, "jane", "update3");
                activities.Items.Add(activity3);

                Result ret = gnipConnection.Publish(base.localPublisher, activities);
                Assert.AreEqual(true, ret.IsSuccess);

                WaitForServerWorkToComplete();

                Activities serverActivities = gnipConnection.GetActivities(localPublisher, existingFilter);
                Assert.IsNotNull(serverActivities);
                Assert.IsNotNull(serverActivities.Items);
                Assert.AreEqual(2, serverActivities.Items.Count);

                //  dates are different
                serverActivities.Items[0].At = activity1.At;
                serverActivities.Items[1].At = activity3.At;

                Assert.IsTrue(activity1.DeepEquals(serverActivities.Items[0]));
                Assert.IsTrue(activity3.DeepEquals(serverActivities.Items[1]));
            }
            finally
            {
                Result res = gnipConnection.Delete(localPublisher, existingFilter);
                Assert.AreEqual(true, res.IsSuccess);
            }
        }

        [Test]
        public void TestGetActivityForFilterFromGnip_03()
        {
            Filter existingFilter = new Filter("existingFilter" + rand.Next(), null, true);
            try
            {
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "joe"));
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "jane"));
                gnipConnection.Create(localPublisher, existingFilter);

                WaitForServerWorkToComplete();

                Activities activities = new Activities();
                Activity activity1 = this.GetActivity2(1, "joe", "person1", "update1");
                activities.Items.Add(activity1);
                Activity activity2 = this.GetActivity2(2, "tom", "person2", "update2");
                activities.Items.Add(activity2);
                Activity activity3 = this.GetActivity2(3, "jane", "person3", "update3");
                activities.Items.Add(activity3);

                Result ret = gnipConnection.Publish(base.localPublisher, activities);
                Assert.AreEqual(true, ret.IsSuccess);

                WaitForServerWorkToComplete();

                Activities serverActivities = gnipConnection.GetActivities(localPublisher, existingFilter);
                Assert.IsNotNull(serverActivities);
                Assert.IsNotNull(serverActivities.Items);
                Assert.AreEqual(2, serverActivities.Items.Count);

                //  dates are different
                serverActivities.Items[0].At = activity1.At;
                serverActivities.Items[1].At = activity3.At;

                Assert.IsTrue(activity1.DeepEquals(serverActivities.Items[0]));
                Assert.IsTrue(activity3.DeepEquals(serverActivities.Items[1]));
            }
            finally
            {
                Result res = gnipConnection.Delete(localPublisher, existingFilter);
                Assert.AreEqual(true, res.IsSuccess);
            }
        }

        [Test]
        public void TestGetActivityForFilterFromGnipWithTime_01()
        {
            // This test (and the 02 version) is failing intermittently. Not sure why.
            Filter existingFilter = new Filter("existingFilter" + rand.Next());

            try
            {
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "joe10"));
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "jane10"));
                gnipConnection.Create(localPublisher, existingFilter);

                WaitForServerWorkToComplete();

                Activities localActivities = new Activities();
                localActivities.Items.Add(new Activity(new Actor("joe10"), "update1"));
                localActivities.Items.Add(new Activity(new Actor("tom10"), "update2"));
                localActivities.Items.Add(new Activity(new Actor("jane10"), "update3"));

                DateTime now = DateTime.Now;
                gnipConnection.Publish(localPublisher, localActivities);

                WaitForServerWorkToComplete();

                Activities remoteActivities = gnipConnection.GetActivities(localPublisher, existingFilter, now);
                Assert.IsNotNull(remoteActivities);
                Assert.AreEqual(2, remoteActivities.Items.Count);
                Assert.AreEqual(localActivities.Items[0].Action, remoteActivities.Items[0].Action);
                Assert.AreEqual(localActivities.Items[2].Action, remoteActivities.Items[1].Action);
            }
            finally
            {
                Result res = gnipConnection.Delete(localPublisher, existingFilter);
                Assert.AreEqual(true, res.IsSuccess);
            }
        }

        [Test]
        public void TestGetActivityForFilterFromGnipWithTime_02()
        {
            // This test (and the 01 version) is failing intermittently. Not sure why.
            Filter existingFilter = new Filter("existingFilter" + rand.Next());

            try
            {
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "joe10"));
                existingFilter.Rules.Add(new Rule(RuleType.Actor, "jane10"));
                gnipConnection.Create(localPublisher, existingFilter);

                WaitForServerWorkToComplete();

                Activities localActivities = new Activities();
                localActivities.Items.Add(new Activity(new Actor("joe10"), "update1"));
                localActivities.Items.Add(new Activity(new Actor("tom10"), "update2"));
                localActivities.Items.Add(new Activity(new Actor("jane10"), "update3"));

                DateTime now = DateTime.Now;
                gnipConnection.Publish(localPublisher, localActivities);

                WaitForServerWorkToComplete();

                Activities remoteActivities = gnipConnection.GetActivities(localPublisher, existingFilter, now);
                Assert.IsNotNull(remoteActivities);
                Assert.AreEqual(2, remoteActivities.Items.Count);
                Assert.AreEqual(localActivities.Items[0].Action, remoteActivities.Items[0].Action);
                Assert.AreEqual(localActivities.Items[2].Action, remoteActivities.Items[1].Action);
            }
            finally
            {
                Result res = gnipConnection.Delete(localPublisher, existingFilter);
                Assert.AreEqual(true, res.IsSuccess);
            }
        }

        public Activity GetActivity1(int val, string actor, string action)
        {
            Activity activity = new Activity();
            activity.At = DateTime.Now;
            activity.Action = action;
            activity.ActivityId = "activityID " + val;
            activity.Url = "http://www.gnip.com/" + val;
            activity.Sources.Add("sources " + val);
            activity.Keywords.Add("keywords " + val);
            activity.Places.Add(new Place(new double[] { 1.2, 3.4 }));
            activity.Actors.Add(new Actor(actor, "Actors.uid " + val, "http://www.google.com/Actors.metaUrl/" + val));
            activity.DestinationUrls.Add(new GnipUrl("http://www.gnip.com/DestinationUrls.value" + val, "http://www.gnip.com/DestinationUrls.metaUrl" + val));
            activity.Tags.Add(new GnipValue("Tags.value " + val, "http://www.gnip.com/Tags.metaUrl" + val));
            activity.Tos.Add(new GnipValue("Tos.value " + val, "http://www.gnip.com/Tos.metaUrl" + val));
            activity.RegardingUrls.Add(new GnipUrl("http://www.gnip.com/RegardingUrls.value" + val, "http://www.gnip.com/RegardingUrls.metaUrl" + val));
            activity.Payload = new Payload("Payload.title " + val, "Payload.body " + val, new List<MediaUrl>() { new MediaUrl("http://www.gnip.com/MediaUrl.url" + val, "width " + val, "height " + val, "duration " + val, "mimeType " + val, "type " + val) }, "Payload.raw " + val, false);

            return activity;
        }

        public Activity GetActivity2(int val, string actor1, string actor2, string action)
        {
            Activity activity = new Activity();

            activity.At = DateTime.Now;
            activity.Action = action;
            activity.ActivityId = "activityID " + (val + 0);
            activity.Url = "http://www.gnip.com/" + (val + 0);
            activity.Sources = new List<string> { "sources " + (val + 0), "sources " + (val + 1) };
            activity.Keywords = new List<string> { "keywords " + (val + 0), "keywords " + (val + 1) };
            activity.Places = new List<Place> { new Place(new double[] { 1.2, 3.4 }), new Place(new double[] { 2.2, 4.4 }) };
            activity.Actors = new List<Actor> {
                new Actor(actor1, "Actors.uid " + (val + 0), "http://www.gnip.com/Actors.metaUrl" + (val + 0)),
                new Actor(actor2, "Actors.uid " + (val + 1), "http://www.gnip.com/Actors.metaUrl" + (val + 1))
            };
            activity.DestinationUrls = new List<GnipUrl> {
                new GnipUrl("http://www.gnip.com/DestinationUrls.value" + (val + 0), "http://www.gnip.com/DestinationUrls.metaUrl" + (val + 0)),
                new GnipUrl("http://www.gnip.com/DestinationUrls.value" + (val + 1), "http://www.gnip.com/DestinationUrls.metaUrl" + (val + 1))
            };
            activity.Tags = new List<GnipValue> {
                new GnipValue("Tags.value " + (val + 0), "http://www.gnip.com/Tags.metaUrl" + (val + 0)),
                new GnipValue("Tags.value " + (val + 1), "http://www.gnip.com/Tags.metaUrl" + (val + 1))
            };
            activity.Tos = new List<GnipValue> {
                new GnipValue("Tos.value " + (val + 0), "http://www.gnip.com/Tos.metaUrl" + (val + 0)),
                new GnipValue("Tos.value " + (val + 1), "http://www.gnip.com/Tos.metaUrl" + (val + 1))
            };
            activity.RegardingUrls = new List<GnipUrl> {
                new GnipUrl("http://www.gnip.com/RegardingUrls.value" + (val + 0), "http://www.gnip.com/RegardingUrls.metaUrl" + (val + 0)),
                new GnipUrl("http://www.gnip.com/RegardingUrls.value" + (val + 1), "http://www.gnip.com/RegardingUrls.metaUrl" + (val + 1))
            };
            activity.Payload = new Payload("Payload.title 1", "Payload.body 1", "Payload.raw 1", false);

            return activity;
        }

		private Activity CreateActivityWithPayload(Payload payload)
		{
			return new Activity(new Actor("jojo"), "some-simple-update", payload);
		}
	}
}