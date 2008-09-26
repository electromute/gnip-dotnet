using System;
using NUnit.Framework;

namespace Gnip.Tests.Unit
{
    [TestFixture]
    public class ActivityTest
    {
        [Test]
        public void CanSerialize()
        {
            Activity a = new Activity();
            a.At = DateTime.Parse("2008-07-01T19:19:36-04:00").ToUniversalTime();
            a.Action = "upload";
            a.Actor = "sally";
            a.Regarding = "blog_post";
            a.Source = "web";
            a.Tags = "trains,planes,automobiles";
            a.To = "bob";
            a.Url = "http://example.com";
            
            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?><activity xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" at=""2008-07-01T23:19:36Z"" action=""upload"" actor=""sally"" regarding=""blog_post"" source=""web"" tags=""trains,planes,automobiles"" to=""bob"" url=""http://example.com"" />",
                            a.ToXml());

            Activities activities = new Activities();
            a.Regarding = "";
            activities.Add(a);

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?><activities xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><activity at=""2008-07-01T23:19:36Z"" action=""upload"" actor=""sally"" regarding="""" source=""web"" tags=""trains,planes,automobiles"" to=""bob"" url=""http://example.com"" /></activities>",
            activities.ToXml());
        }

        [Test]
        public void CanDeserialize()
        {
            Activities activities = UTF8XmlSerializer.Deserialize<Activities>("<activities>" +
                "<activity at='2008-07-01T23:19:36Z' action='upload' actor='sally' regarding='blog_post' source='web' tags='trains,planes,automobiles' to='bob' url='http://example.com' />" +
                "<activity at='2008-07-01T23:19:37Z' action='upload' actor='sally'/>" +
                "</activities>");

            Assert.AreEqual(2, activities.Count);
            Assert.AreEqual("sally", activities[1].Actor);
            Assert.IsNull(activities[1].Source);
        }

        [Test]
        public void EqualitySemantics()
        {
            DateTime now = DateTime.Now;

            Assert.AreEqual(new Activity(now, "upload", "sally", "blog_post", "web", "trains,planes,automobiles", "bob", "http://example.com"),
                            new Activity(now, "upload", "sally", "blog_post", "web", "trains,planes,automobiles", "bob", "http://example.com"));
            Assert.AreNotEqual(new Activity(now, "upload", "sally", "blog_post", "web", "trains,planes,automobiles", "bob", "http://example.com"),
                               new Activity(now, "upload", "sally", "blog_post", "web", "trains,planes,automobiles", "fred", "http://example.com"));
        }
    }
}
