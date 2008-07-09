using System.Xml.Serialization;

namespace Gnip
{
    public class Resource
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlIgnore]
        public string Url
        {
            get { return string.Format("/{0}s/{1}", GetType().Name.ToLower(), Name); }
        }
    }
}
