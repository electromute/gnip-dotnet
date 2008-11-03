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
        public string PostUrl;  
      
        [XmlElement("rule")]
        public List<Rule> Rules;

        private Filter() { }

        public Filter(string name, string fullData, string postUrl, List<Rule> rules)
        {
            this.Name = name;
            this.FullData = fullData;
            this.Rules = rules;
            this.PostUrl = postUrl;
        }

        public void SetPostUrl(string postUrl)
        {
            this.PostUrl = postUrl;
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
