using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnip
{
    public interface IConnection
    {
        Publishers GetPublishers();
        
        Publisher GetPublisher(string name);
        Filter GetFilter(string publisher, string name);
        
        void Create(Publisher publisher);
        void Create(string publisher, Filter filter);

        void Update(string publisher, Filter filter);

        void Delete(string publisher, string filter);
        
        void Publish(string publisher, Activities activities);

        Activities GetPublisherActivities(String publisher);
        Activities GetPublisherActivities(String publisher, DateTime bucket);
        Activities GetPublisherNotifications(String publisher);
        Activities GetPublisherNotifications(String publisher, DateTime bucket);
        Activities GetFilterActivities(String publisher, String filter);
        Activities GetFilterActivities(String publisher, String filter, DateTime bucket);
        Activities GetFilterNotifications(String publisher, String filter);
        Activities GetFilterNotifications(String publisher, String filter, DateTime bucket);
    }
}
