using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gnip
{
    public class Connection : IConnection
    {
        private IServer server;

        public Connection(string username, string password) : this(new Server(username, password)) { }

        public Connection(IServer server)
        {
            this.server = server;
        }

        public Publishers GetPublishers()
        {
            return GetPublishers("/publishers.xml");
        }

        public Publisher GetPublisher(string name)
        {
            return UTF8XmlSerializer.Deserialize<Publisher>(server.Get(new Publisher(name).Url + ".xml"));
        }

        public Collection GetCollection(string name)
        {
            return UTF8XmlSerializer.Deserialize<Collection>(server.Get(new Collection(name).Url + ".xml"));
        }

        public void Create(Publisher publisher)
        {
            server.Post("/publishers.xml", publisher.ToXml());
        }

        public void Create(Collection collection)
        {
            server.Post("/collections.xml", collection.ToXml());
        }

        public void Delete(Collection collection)
        {
            server.Delete(collection.Url + ".xml");
        }

        public void Update(Collection collection)
        {
            server.Put(collection.Url + ".xml", collection.ToXml());
        }

        public Activities GetActivities(Resource resource)
        {
            return GetActivities(resource.Url + "/activity/current.xml");
        }

        public Activities GetActivities(Resource resource, DateTime time)
        {
            return GetActivities(string.Format("{0}/activity/{1}.xml", resource.Url, GetBucket(time)));
        }

        public void Publish(Publisher publisher, Activities activities)
        {
            server.Post(publisher.Url + "/activity.xml", activities.ToXml());
        }


        protected Activities GetActivities(string url)
        {
            return UTF8XmlSerializer.Deserialize<Activities>(server.Get(url));
        }

        protected Publishers GetPublishers(string url)
        {
            return UTF8XmlSerializer.Deserialize<Publishers>(server.Get(url));
        }

        protected string GetBucket(DateTime time)
        {
            DateTime flooredDate = FiveMinuteFloor(time.ToUniversalTime());
            return flooredDate.ToString("yyyyMMddHHmm");
        }

        private static DateTime FiveMinuteFloor(DateTime time)
        {
            long floor = time.Ticks / FIVE_MINUTES;
            return new DateTime(floor * FIVE_MINUTES, DateTimeKind.Utc);
        }

        private const long FIVE_MINUTES = 5 * TimeSpan.TicksPerMinute;
    }
}
