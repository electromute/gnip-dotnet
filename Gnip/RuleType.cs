using System.Xml;
using System.Xml.Serialization;

namespace Gnip
{
    [XmlType("type")]
    public class RuleType
    {
        [XmlText]
        public string Type;
    
        private RuleType() { }

        public RuleType(string type)
        {
            this.Type = type;
        }

        public override bool Equals(object obj)
        {
            return obj is RuleType &&
                ((RuleType)obj).Type == Type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public string ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }
}
