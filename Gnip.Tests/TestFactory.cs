using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace Gnip.Tests
{
    class TestFactory
    {
        public static Publisher ExistingPublisher = new Publisher("", new RuleTypes() { new RuleType("actor") });
        public static Connection LiveConnection()
        {
            return new Connection("", "");
        }

        public static Activities Activities()
        {
            Activities activities = UTF8XmlSerializer.Deserialize<Activities>(@"<activities><activity at=""2008-07-01T23:19:36Z"" action=""upload"" actor=""sally"" regarding="""" source=""web"" tags=""trains,planes,automobiles"" to=""bob"" url=""http://example.com"" /><activity at=""2008-07-01T23:19:37Z"" action=""upload"" actor=""sally"" regarding="""" source=""web"" tags=""trains,planes,automobiles"" to=""bob"" url=""http://example.com"" /></activities>");

            string randIntString = (new Random()).Next(0, 99999999).ToString();
            activities[0].Regarding = randIntString;
            activities[1].Regarding = randIntString;
            
            return activities;
        }

        public static Filter Filter()
        {
            return UTF8XmlSerializer.Deserialize<Filter>(@"<filter name='example1' fullData='false'><rule type='actor' value='me'/><rule type='actor' value='you'/></filter>");
        }
    }
}
