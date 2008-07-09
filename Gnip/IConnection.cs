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
        Collection GetCollection(string name);
        
        void Create(Publisher publisher);
        void Create(Collection collection);

        void Update(Collection collection);

        void Delete(Collection collection);
        
        void Publish(Publisher publisher, Activities activities);
        
        Activities GetActivities(Resource resource);
        Activities GetActivities(Resource resource, DateTime bucket);
    }
}
