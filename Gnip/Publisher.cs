using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gnip
{
    [XmlType("publishers")]
    public class Publishers : List<Publisher>
    {
        public string ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }

    [XmlType("publisher")]
    public class Publisher : Resource
    {
        public Publisher(string name)
        {
            this.Name = name;
        }

        private Publisher() {}

        public override bool Equals(object obj)
        {
            return obj is Publisher && ((Publisher)obj).Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return "Publisher<" + Name + ">";
        }

        public string ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }
}
