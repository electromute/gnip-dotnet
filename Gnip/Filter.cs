using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Gnip
{
    [XmlType("filter")]
    public class Filter
    {
        [XmlAttribute("name")]
        public string Name; 

        [XmlAttribute("fullData")]
        public string FullData; 
       
        [XmlElement("postUrl")]
        [XmlElement("jid")]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string OptionalItem;  

        [XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName;
      
        [XmlElement("rule")]
        public List<Rule> Rules;

        private Filter() { }

        public Filter(string name, string fullData, params Rule [] uids)
        {
            this.Name = name;
            this.FullData = fullData;
            this.Rules = new List<Rule>(uids);
        }

        public void SetJid(string jid)
        {
            this.OptionalItem = jid;
            this.ItemElementName = ItemChoiceType.jid;
        }

        public void SetPostUrl(string postUrl)
        {
            this.OptionalItem = postUrl;
            this.ItemElementName = ItemChoiceType.postUrl;
        }

        public enum ItemChoiceType
        {
            postUrl,
            jid
        }

        public override bool Equals(object obj)
        {
            return obj is Filter && 
                ((Filter)obj).Name == Name &&
                Equals(((Filter)obj).Rules, Rules);
        }

        private bool Equals(List<Rule> a, List<Rule> b)
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
