using System.Xml;
using System.Xml.Serialization;

namespace Gnip
{
    [XmlType("uid")]
    public class Uid
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlAttribute("publisher.name")]
        public string PublisherName;
        [XmlIgnore]
        public Publisher Publisher
        {
            get { return new Publisher(this.PublisherName); }
            set { this.PublisherName = value.Name; }
        }

        private Uid() { }

        public Uid(Publisher publisher, string name)
        {
            this.Publisher = publisher;
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is Uid &&
                ((Uid)obj).Name == Name &&
                ((Uid)obj).PublisherName == PublisherName;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public string ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }
}
