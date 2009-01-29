using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class ActivitiesTest : BaseResourceTest
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
        public void TestActivityConstructor_01()
        {
            DateTime now = DateTime.Now;

            Activities activities = new Activities();
            activities.Publisher = "foo";
            activities.Items.Add(ActivityTest.GetActivity1(1, now));
            activities.Items.Add(ActivityTest.GetActivity1(2, now));
            activities.Items.Add(ActivityTest.GetActivity1(3, now));

            Assert.AreEqual("foo", activities.Publisher);
            for (int idx = 0; idx < activities.Items.Count; idx++)
            {
                Assert.IsTrue(ActivityTest.GetActivity1(idx + 1, now).DeepEquals(activities.Items[idx]));
            }
        }

        [Test]
        public void TestActivitySerialize_01()
        {
            DateTime date = new DateTime(2002, 1, 1);
            Activities activities = new Activities();
            activities.Publisher = "foo";

            string str = XmlHelper.Instance.ToXmlString<Activities>(activities);

            //Console.WriteLine(str);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<activities xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" publisher=\"foo\" />",
                            str);
        }

        [Test]
        public void TestActivitySerialize_02()
        {
            DateTime date = new DateTime(2002, 1, 1);
            Activities activities = new Activities();
            activities.Publisher = "foo";
            activities.Items.Add(ActivityTest.GetActivity1(1, date));
            activities.Items.Add(ActivityTest.GetActivity1(2, date));
            activities.Items.Add(ActivityTest.GetActivity1(3, date));

            string str = XmlHelper.Instance.ToXmlString<Activities>(activities);

            //Console.WriteLine(str);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<activities xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" publisher=\"foo\">" +
                            "<activity>" +
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
                            "</activity>" +
                            "<activity>" +
                                "<at>2002-01-01T00:00:00</at>" +
                                "<action>action 2</action>" +
                                "<activityID>activityID 2</activityID>" +
                                "<URL>url 2</URL>" +
                                "<source>sources 2</source>" +
                                "<keyword>keywords 2</keyword>" +
                                "<place>" +
                                    "<point>1.2 3.4</point>" +
                                "</place>" +
                                "<actor metaURL=\"Actors.metaUrl 2\" uid=\"Actors.uid 2\">Actors.value 2</actor>" +
                                "<destinationURL metaURL=\"DestinationUrls.metaUrl 2\">DestinationUrls.value 2</destinationURL>" +
                                "<tag metaURL=\"Tags.metaUrl 2\">Tags.value 2</tag>" +
                                "<to metaURL=\"Tos.metaUrl 2\">Tos.value 2</to>" +
                                "<regardingURL metaURL=\"RegardingUrls.metaUrl 2\">RegardingUrls.value 2</regardingURL>" +
                                "<payload>" +
                                    "<title>Payload.title 1</title>" +
                                    "<body>Payload.body 1</body>" +
                                    "<raw>H4sIAAAAAAAEAO29B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Il5m12WVzcZ1dpXu/j94QhWzDQAAAA==</raw>" +
                                "</payload>" +
                            "</activity>" +
                            "<activity>" +
                                "<at>2002-01-01T00:00:00</at>" +
                                "<action>action 3</action>" +
                                "<activityID>activityID 3</activityID>" +
                                "<URL>url 3</URL>" +
                                "<source>sources 3</source>" +
                                "<keyword>keywords 3</keyword>" +
                                "<place>" +
                                    "<point>1.2 3.4</point>" +
                                "</place>" +
                                "<actor metaURL=\"Actors.metaUrl 3\" uid=\"Actors.uid 3\">Actors.value 3</actor>" +
                                "<destinationURL metaURL=\"DestinationUrls.metaUrl 3\">DestinationUrls.value 3</destinationURL>" +
                                "<tag metaURL=\"Tags.metaUrl 3\">Tags.value 3</tag>" +
                                "<to metaURL=\"Tos.metaUrl 3\">Tos.value 3</to>" +
                                "<regardingURL metaURL=\"RegardingUrls.metaUrl 3\">RegardingUrls.value 3</regardingURL>" +
                                "<payload>" +
                                    "<title>Payload.title 1</title>" +
                                    "<body>Payload.body 1</body>" +
                                    "<raw>H4sIAAAAAAAEAO29B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Il5m12WVzcZ1dpXu/j94QhWzDQAAAA==</raw>" +
                                "</payload>" +
                            "</activity>" +
                            "</activities>",
                            str);
        }

        [Test]
        public void TestActivityDeserialize_01()
        {
            DateTime date = new DateTime(2002, 1, 1);
            Activities activities = new Activities();
            activities.Publisher = "foo";
            activities.Items.Add(ActivityTest.GetActivity1(1, date));
            activities.Items.Add(ActivityTest.GetActivity1(2, date));
            activities.Items.Add(ActivityTest.GetActivity1(3, date));

            string str = XmlHelper.Instance.ToXmlString<Activities>(activities);
            Activities des = XmlHelper.Instance.FromXmlString<Activities>(str);
            Assert.AreEqual(3, des.Items.Count);
            for (int idx = 0; idx < des.Items.Count; idx++)
            {
                Assert.IsTrue(activities.Items[idx].DeepEquals(des.Items[idx]));
            }
        }
    }
}
