using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Resource;
using Gnip.Client.Utils;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class ActivityTest : BaseResourceTest
    {
        public static Activity GetActivity1(int val, DateTime at)
        {
            Activity activity = new Activity();

            activity.At = at;
            activity.Action = "action " + val;
            activity.ActivityId = "activityID " + val;
            activity.Url = "url " + val;
            activity.Sources.Add("sources " + val);
            activity.Keywords.Add("keywords " + val);
            activity.Places.Add(new Place(new double[] { 1.2, 3.4 }));
            activity.Actors.Add(new Actor("Actors.value " + val, "Actors.uid " + val, "Actors.metaUrl " + val));
            activity.DestinationUrls.Add(new GnipUrl("DestinationUrls.value " + val, "DestinationUrls.metaUrl " + val));
            activity.Tags.Add(new GnipValue("Tags.value " + val, "Tags.metaUrl " + val));
            activity.Tos.Add(new GnipValue("Tos.value " + val, "Tos.metaUrl " + val));
            activity.RegardingUrls.Add(new GnipUrl("RegardingUrls.value " + val, "RegardingUrls.metaUrl " + val));
            activity.Payload = new Payload("Payload.title 1", "Payload.body 1", "Payload.raw 1", false);

            return activity;
        }

        public static Activity GetActivity2(int val, DateTime at)
        {
            Activity activity = new Activity();

            activity.At = at;
            activity.Action = "action " + (val + 0);
            activity.ActivityId = "activityID " + (val + 0);
            activity.Url = "url " + (val + 0);
            activity.Sources = new List<string> { "sources " + (val + 0), "sources " + (val + 1) };
            activity.Keywords = new List<string> { "keywords " + (val + 0), "keywords " + (val + 1) };
            activity.Places = new List<Place> { new Place(new double[] { 1.2, 3.4 }), new Place(new double[] { 2.2, 4.4 }) };
            activity.Actors = new List<Actor> {
                new Actor("Actors.value " + (val + 0), "Actors.uid " + (val + 0), "Actors.metaUrl " + (val + 0)),
                new Actor("Actors.value " + (val + 1), "Actors.uid " + (val + 1), "Actors.metaUrl " + (val + 1))
            };
            activity.DestinationUrls = new List<GnipUrl> {
                new GnipUrl("DestinationUrls.value " + (val + 0), "DestinationUrls.metaUrl " + (val + 0)),
                new GnipUrl("DestinationUrls.value " + (val + 1), "DestinationUrls.metaUrl " + (val + 1))
            };
            activity.Tags = new List<GnipValue> {
                new GnipValue("Tags.value " + (val + 0), "Tags.metaUrl " + (val + 0)),
                new GnipValue("Tags.value " + (val + 1), "Tags.metaUrl " + (val + 1))
            };
            activity.Tos = new List<GnipValue> {
                new GnipValue("Tos.value " + (val + 0), "Tos.metaUrl " + (val + 0)),
                new GnipValue("Tos.value " + (val + 1), "Tos.metaUrl " + (val + 1))
            };
            activity.RegardingUrls = new List<GnipUrl> {
                new GnipUrl("RegardingUrls.value " + (val + 0), "RegardingUrls.metaUrl " + (val + 0)),
                new GnipUrl("RegardingUrls.value " + (val + 1), "RegardingUrls.metaUrl " + (val + 1))
            };
            activity.Payload = new Payload(
                "Payload.title " + (val + 0),
                "Payload.body " + (val + 0),
                new List<MediaUrl>() { new MediaUrl("url " + (val + 0), "width " + (val + 0), "height " + (val + 0), "duration " + (val + 0), "mimeType " + (val + 0), "type " + (val + 0)) },
                "Payload.raw " + (val + 0), 
                false);

            return activity;
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestActivityConstructor_01()
        {
            DateTime now = DateTime.Now;

            int val = 1;

            Activity activity = ActivityTest.GetActivity1(1, now);
            Assert.AreEqual(now, activity.At);
            Assert.AreEqual("action " + val, activity.Action);
            Assert.AreEqual("activityID " + val, activity.ActivityId);
            Assert.AreEqual("url " + val, activity.Url);
            Assert.IsTrue(ListUtils.AreEqual<string>(new List<string> {"sources " + val}, activity.Sources));
            Assert.IsTrue(ListUtils.AreEqual<string>(new List<string> { "keywords " + val }, activity.Keywords));
            Assert.AreEqual(1.2, activity.Places[0].Point[0]);
            Assert.AreEqual(3.4, activity.Places[0].Point[1]);
            Assert.IsTrue(ListUtils.AreEqual<Actor>(new List<Actor> { new Actor("Actors.value " + val, "Actors.uid " + val, "Actors.metaUrl " + val) }, activity.Actors));
            Assert.IsTrue(ListUtils.AreEqual<GnipUrl>(new List<GnipUrl> { new GnipUrl("DestinationUrls.value " + val, "DestinationUrls.metaUrl " + val) }, activity.DestinationUrls));
            Assert.IsTrue(ListUtils.AreEqual<GnipValue>(new List<GnipValue> { new GnipValue("Tags.value " + val, "Tags.metaUrl " + val) }, activity.Tags));
            Assert.IsTrue(ListUtils.AreEqual<GnipValue>(new List<GnipValue> { new GnipValue("Tos.value " + val, "Tos.metaUrl " + val) }, activity.Tos));
            Assert.IsTrue(ListUtils.AreEqual<GnipUrl>(new List<GnipUrl> { new GnipUrl("RegardingUrls.value " + val, "RegardingUrls.metaUrl " + val) }, activity.RegardingUrls));
            Assert.IsTrue(ObjectUtils.AreDeepEqual(new Payload("Payload.title 1", "Payload.body 1", "Payload.raw 1", false), activity.Payload));        
        }

        private void TestDeepEquals(Activity objectA, Activity objectB, bool expect, bool expectDeep)
        {
            objectA.At = objectB.At;

            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestActivityConstructor_02()
        {
            DateTime now = DateTime.Now;

            this.TestDeepEquals(
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1"),
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1"),
                false, true);

            this.TestDeepEquals(
                new Activity(new Actor("value 2", "uid 1", "metaUrl 1"), "action 1"),
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1"),
                false, false);

            this.TestDeepEquals(
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 2"),
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1"),
                false, false);
        }

        [Test]
        public void TestActivityConstructor_03()
        {
            this.TestDeepEquals(
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1", new Payload("title 1", "body 1", "value 1", false)),
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1", new Payload("title 1", "body 1", "value 1", false)),
                false, true);

            this.TestDeepEquals(
                new Activity(new Actor("value 2", "uid 2", "metaUrl 2"), "action 1", new Payload("title 1", "body 1", "value 1", false)),
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1", new Payload("title 1", "body 1", "value 1", false)),
                false, false);

            this.TestDeepEquals(
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 2", new Payload("title 1", "body 1", "value 1", false)),
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1", new Payload("title 1", "body 1", "value 1", false)),
                false, false);

            this.TestDeepEquals(
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1", new Payload("title 1", "body 2", "value 2", false)),
                new Activity(new Actor("value 1", "uid 1", "metaUrl 1"), "action 1", new Payload("title 1", "body 1", "value 1", false)),
                false, false);
        }

        [Test]
        public void TestActivitySerialize_01()
        {
            DateTime date = new DateTime(2002, 1, 1);

            Activity Activity = ActivityTest.GetActivity1(1, date);

            string str = XmlHelper.Instance.ToXmlString<Activity>(Activity);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<activity xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                                "<at>2002-01-01T00:00:00</at>" +
                                "<action>action 1</action>" +
                                "<activityID>activityID 1</activityID>" +
                                "<URL>url 1</URL>" +
                                "<source>sources 1</source>" +
                                "<keyword>keywords 1</keyword>" +
                                "<place>" +
                                    "<point>1.2 3.4</point>" +
                                "</place>" +
                                "<actor metaURL=\"Actors.metaUrl 1\" uid=\"Actors.uid 1\">Actors.value 1</actor>" +
                                "<destinationURL metaURL=\"DestinationUrls.metaUrl 1\">DestinationUrls.value 1</destinationURL>" +
                                "<tag metaURL=\"Tags.metaUrl 1\">Tags.value 1</tag>" +
                                "<to metaURL=\"Tos.metaUrl 1\">Tos.value 1</to>" +
                                "<regardingURL metaURL=\"RegardingUrls.metaUrl 1\">RegardingUrls.value 1</regardingURL>" +
                                "<payload>" +
                                    "<title>Payload.title 1</title>" +
                                    "<body>Payload.body 1</body>" +
                                    "<raw>H4sIAAAAAAAEAO29B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Il5m12WVzcZ1dpXu/j94QhWzDQAAAA==</raw>" +
                                "</payload>" +
                            "</activity>",
                            str);
        }

        [Test]
        public void TestActivitySerialize_02()
        {
            DateTime date = new DateTime(2002, 1, 1);

            Activity Activity = ActivityTest.GetActivity2(1, date);

            string str = XmlHelper.Instance.ToXmlString<Activity>(Activity);
            Assert.AreEqual(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<activity xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                    "<at>2002-01-01T00:00:00</at>" +
                    "<action>action 1</action>" +
                    "<activityID>activityID 1</activityID>" +
                    "<URL>url 1</URL>" +
                    "<source>sources 1</source>" +
                    "<source>sources 2</source>" +
                    "<keyword>keywords 1</keyword>" +
                    "<keyword>keywords 2</keyword>" +
                    "<place>" +
                        "<point>1.2 3.4</point>" +
                    "</place>" +
                    "<place>" +
                        "<point>2.2 4.4</point>" +
                    "</place>" +
                    "<actor metaURL=\"Actors.metaUrl 1\" uid=\"Actors.uid 1\">Actors.value 1</actor>" +
                    "<actor metaURL=\"Actors.metaUrl 2\" uid=\"Actors.uid 2\">Actors.value 2</actor>" +
                    "<destinationURL metaURL=\"DestinationUrls.metaUrl 1\">DestinationUrls.value 1</destinationURL>" +
                    "<destinationURL metaURL=\"DestinationUrls.metaUrl 2\">DestinationUrls.value 2</destinationURL>" +
                    "<tag metaURL=\"Tags.metaUrl 1\">Tags.value 1</tag>" +
                    "<tag metaURL=\"Tags.metaUrl 2\">Tags.value 2</tag>" +
                    "<to metaURL=\"Tos.metaUrl 1\">Tos.value 1</to>" +
                    "<to metaURL=\"Tos.metaUrl 2\">Tos.value 2</to>" +
                    "<regardingURL metaURL=\"RegardingUrls.metaUrl 1\">RegardingUrls.value 1</regardingURL>" +
                    "<regardingURL metaURL=\"RegardingUrls.metaUrl 2\">RegardingUrls.value 2</regardingURL>" +
                    "<payload>" +
                        "<title>Payload.title 1</title>" +
                        "<body>Payload.body 1</body>" +
                        "<mediaURL height=\"height 1\" width=\"width 1\" duration=\"duration 1\" mimeType=\"mimeType 1\" type=\"type 1\">url 1</mediaURL>" +
                        "<raw>H4sIAAAAAAAEAO29B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Il5m12WVzcZ1dpXu/j94QhWzDQAAAA==</raw>" +
                    "</payload>" +
                "</activity>"
                , str);
        }

        [Test]
        public void TestActivitySerialize_03()
        {
            DateTime date = new DateTime(2002, 1, 1);

            Activity activity = new Activity(date, "action 1");
            string str = XmlHelper.Instance.ToXmlString<Activity>(activity);
            Assert.AreEqual(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<activity xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                    "<at>2002-01-01T00:00:00</at>" +
                    "<action>action 1</action>" +
                "</activity>"
                , str);
        }

        [Test]
        public void TestActivityDeserialize_01()
        {
            Activity activity = ActivityTest.GetActivity1(1, DateTime.Now);

            string str = XmlHelper.Instance.ToXmlString<Activity>(activity);
            Activity des = XmlHelper.Instance.FromXmlString<Activity>(str);
            Assert.IsTrue(activity.DeepEquals(des));
        }

        [Test]
        public void TestActivityDeserialize_02()
        {
            DateTime date = new DateTime(2002, 1, 1);

            Activity activity = new Activity(date, "action 1");

            string str = XmlHelper.Instance.ToXmlString<Activity>(activity);
            Activity des = XmlHelper.Instance.FromXmlString<Activity>(str);
            Assert.IsTrue(activity.DeepEquals(des));
        }
    }
}
