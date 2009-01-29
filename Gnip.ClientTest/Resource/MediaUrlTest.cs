using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class MediaUrlTest : BaseResourceTest
    {
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
        public void TestMediaUrlConstructor_01()
        {
            MediaUrl mediaUrl = new MediaUrl();
            mediaUrl.Url = "url 1";

            MediaUrl mediaUrl2 = new MediaUrl(
                "url 1");

            Assert.IsTrue(mediaUrl.DeepEquals(mediaUrl2));

            mediaUrl = new MediaUrl();
            mediaUrl.Url = "url 1";
            mediaUrl.Width = "width 1";
            mediaUrl.Height = "height 1";
            mediaUrl.Duration = "duration 1";
            mediaUrl.MimeType = "mimeType 1";
            mediaUrl.Type = "type 1";

            mediaUrl2 = new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1");

            Assert.IsTrue(mediaUrl.DeepEquals(mediaUrl2));
        }

        private void TestDeepEquals(MediaUrl objectA, MediaUrl objectB, bool expect, bool expectDeep)
        {
            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestMediaUrlConstructor_02()
        {
            this.TestDeepEquals(
                new MediaUrl("url 1"),
                new MediaUrl("url 1"),
                true, true);

            this.TestDeepEquals(
                new MediaUrl("url 2"),
                new MediaUrl("url 1"),
                false, false);
        }

        [Test]
        public void TestMediaUrlConstructor_04()
        {
            this.TestDeepEquals(
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1"),
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1"),
                true, true);

            this.TestDeepEquals(
                new MediaUrl("url 2", "width 1", "height 1", "duration 1", "mimeType 1", "type 1"),
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1"),
                false, false);

            this.TestDeepEquals(
                new MediaUrl("url 1", "width 2", "height 1", "duration 1", "mimeType 1", "type 1"),
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1"),
                false, false);

            this.TestDeepEquals(
                new MediaUrl("url 1", "width 1", "height 2", "duration 1", "mimeType 1", "type 1"),
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1"),
                false, false);

            this.TestDeepEquals(
                new MediaUrl("url 1", "width 1", "height 1", "duration 2", "mimeType 1", "type 1"),
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1"),
                false, false);

            this.TestDeepEquals(
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 2", "type 1"),
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1"),
                false, false);

            this.TestDeepEquals(
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 2"),
                new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1"),
                false, false);
        }

        [Test]
        public void TestMediaUrlSerialize_01()
        {
            MediaUrl mediaUrl = new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1");

            string str = XmlHelper.Instance.ToXmlString<MediaUrl>(mediaUrl);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<mediaURL xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" height=\"height 1\" width=\"width 1\" duration=\"duration 1\" mimeType=\"mimeType 1\" type=\"type 1\">url 1</mediaURL>",
                            str);
        }

        [Test]
        public void TestMediaUrlDeserialize_01()
        {
            MediaUrl mediaUrl = new MediaUrl("url 1", "width 1", "height 1", "duration 1", "mimeType 1", "type 1");

            string str = XmlHelper.Instance.ToXmlString<MediaUrl>(mediaUrl);

            MediaUrl des = XmlHelper.Instance.FromXmlString<MediaUrl>(str);
            Assert.IsTrue(mediaUrl.DeepEquals(des));
        }
    }
}
