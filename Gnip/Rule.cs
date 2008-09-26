using System.Xml;
using System.Xml.Serialization;

namespace Gnip
{
    [XmlType("rule")]
    public class Rule
    {
        [XmlAttribute("type")]
        public string Type;

        [XmlAttribute("value")]
        public string Value;

        private Rule() { }

        public Rule(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public override bool Equals(object obj)
        {
            return obj is Rule &&
                ((Rule)obj).Type == Type &&
                ((Rule)obj).Value == Value;
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
