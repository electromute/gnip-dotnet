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

        public Publisher GetPublisher(string publisher)
        {
            return UTF8XmlSerializer.Deserialize<Publisher>(server.Get("/publishers/" + publisher + ".xml"));
        }

        public Filter GetFilter(string publisher, string filter)
        {
            return UTF8XmlSerializer.Deserialize<Filter>(server.Get("/publishers/" + publisher + "/filters/" + filter + ".xml"));
        }

        public void Create(Publisher publisher)
        {
            server.Post("/publishers.xml", publisher.ToXml());
        }

        public void Create(string publisher, Filter filter)
        {
            server.Post("/publishers/" + publisher + "/filters.xml", filter.ToXml());
        }

        public void Update(string publisher, Filter filter)
        {
            server.Put("/publishers/" + publisher + "/filters/" + filter.Name + ".xml", filter.ToXml());
        }

        public void Delete(string publisher, string filter)
        {
            server.Delete("/publishers/" + publisher + "/filters/" + filter + ".xml");
        }

        public void Publish(string publisher, Activities activities)
        {
            server.Post("/publishers/" + publisher + "/activity.xml", activities.ToXml());
        }

        public Activities GetPublisherActivities(string publisher)
        {
            return GetActivities("/publishers/" + publisher + "/activity/current.xml");
        }

        public Activities GetPublisherActivities(string publisher, DateTime bucket)
        {
            return GetActivities(string.Format("{0}/activity/{1}.xml", "/publishers/" + publisher, GetBucket(bucket)));
        }

        public Activities GetPublisherNotifications(string publisher)
        {
            return GetActivities("/publishers/" + publisher + "/notification/current.xml");
        }

        public Activities GetPublisherNotifications(string publisher, DateTime bucket)
        {
            return GetActivities(string.Format("{0}/notification/{1}.xml", "/publishers/" + publisher, GetBucket(bucket)));
        }

        public Activities GetFilterActivities(string publisher, string filter)
        {
            return GetActivities("/publishers/" + publisher + "/filters/" + filter + "/activity/current.xml");
        }

        public Activities GetFilterActivities(string publisher, string filter, DateTime bucket)
        {
            return GetActivities(string.Format("{0}/activity/{1}.xml", "/publishers/" + publisher + "/filters/" + filter, GetBucket(bucket)));
        }

        public Activities GetFilterNotifications(string publisher, string filter)
        {
            return GetActivities("/publishers/" + publisher + "/filters/" + filter + "/notification/current.xml");
        }

        public Activities GetFilterNotifications(string publisher, string filter, DateTime bucket)
        {
            return GetActivities(string.Format("{0}/notification/{1}.xml", "/publishers/" + publisher + "/filters/" + filter, GetBucket(bucket)));
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
            return time.ToUniversalTime().ToString("yyyyMMddHHmm");
        }
    }
}
