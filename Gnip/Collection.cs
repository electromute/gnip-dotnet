using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Gnip
{
    [XmlType("collection")]
    public class Collection : Resource
    {
        [XmlElement("uid")]
        public List<Uid> Uids;

        private Collection() { }

        public Collection(string name, params Uid [] uids)
        {
            this.Name = name;
            this.Uids = new List<Uid>(uids);
        }

        public override bool Equals(object obj)
        {
            return obj is Collection && 
                ((Collection)obj).Name == Name &&
                Equals(((Collection)obj).Uids, Uids);
        }

        private bool Equals(List<Uid> a, List<Uid> b)
        {
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (!a[i].Equals(b[i])) return false;
            }
            return true;
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
