using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Gnip
{
    [XmlType("activities")]
    public class Activities : List<Activity>
    {
        public string ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }

    [XmlType("activity")]
    public class Activity
    {
        [XmlAttribute("uid")]
        public string Uid;

        [XmlAttribute("at")]
        public string At;

        [XmlAttribute("guid")]
        public string Guid;

        [XmlAttribute("type")]
        public string Type;

        private string publisherName;
        [XmlIgnore]
        public Publisher Publisher
        {
            get { return new Publisher(publisherName); }
            set { publisherName = value.Name; }
        }

        public Activity() { }

        public Activity(string uid, string at, string guid, string type, Publisher publisher)
        {
            this.Uid = uid;
            this.At = at;
            this.Guid = guid;
            this.Publisher = publisher;
        }

        public override bool Equals(object obj)
        {
            return obj is Activity &&
                ((Activity)obj).Uid == Uid &&
                ((Activity)obj).At == At &&
                ((Activity)obj).Guid == Guid &&
                ((Activity)obj).Type == Type &&
                ((Activity)obj).publisherName == publisherName;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public String ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }
}
