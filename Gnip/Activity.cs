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
        [XmlAttribute("at")]
        public DateTime At;

        [XmlAttribute("action")]
        public string Action;

        [XmlAttribute("actor")]
        public string Actor;

        [XmlAttribute("regarding")]
        public string Regarding;

        [XmlAttribute("source")]
        public string Source;

        [XmlAttribute("tags")]
        public string Tags;

        [XmlAttribute("to")]
        public string To;

        [XmlAttribute("url")]
        public string Url;

        public Activity() { }

        public Activity(DateTime at, string action, string actor, string regarding, string source, string tags, string to, string url)
        {
            this.At = at;
            this.Action = action;
            this.Actor = actor;
            this.Regarding = regarding;
            this.Source = source;
            this.Tags = tags;
            this.To = to;
            this.Url = url;
        }

        public override bool Equals(object obj)
        {
            return obj is Activity &&
                ((Activity)obj).At == At &&
                ((Activity)obj).Action == Action &&
                ((Activity)obj).Actor == Actor &&
                ((Activity)obj).Regarding == Regarding &&
                ((Activity)obj).Source == Source &&
                ((Activity)obj).Tags == Tags &&
                ((Activity)obj).To == To &&
                ((Activity)obj).Url == Url;
        }

        public override int GetHashCode()
        {
            return At.GetHashCode() + 
                Action.GetHashCode() +
                Actor.GetHashCode() +
                Regarding.GetHashCode() +
                Source.GetHashCode() +
                Tags.GetHashCode() +
                To.GetHashCode() + 
                Url.GetHashCode();
        }

        public String ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }
}
