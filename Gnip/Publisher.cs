using System.Collections.Generic;
using System.Xml;
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

    [XmlType("supportedRuleTypes")]
    public class RuleTypes : List<RuleType>
    {
        public string ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }

    [XmlType("publisher")]
    public class Publisher
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlArray("supportedRuleTypes")]
        public RuleTypes RuleTypes;

        private Publisher() { }

        public Publisher(string name, RuleTypes ruleTypes)
        {
            this.Name = name;
            this.RuleTypes = ruleTypes;
        }

        public override bool Equals(object obj)
        {
            return obj is Publisher &&
                ((Publisher)obj).Name == Name &&
                Equals(((Publisher)obj).RuleTypes, RuleTypes);
        }

        private bool Equals(RuleTypes a, RuleTypes b)
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
