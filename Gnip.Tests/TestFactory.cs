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
        // todo - replace this with not my user / pass :)
        public static Publisher ExistingPublisher = new Publisher("bob");
        public static Connection LiveConnection()
        {
            return new Connection("jeremy.lightsmith@gmail.com", "test");
        }

        public static Guid TestRunGuid = Guid.NewGuid();

        public static Activities Activities()
        {
            return UTF8XmlSerializer.Deserialize<Activities>(
@"<activities>
    <activity at='2008-07-01T07:19:45-04:00' guid='152481660' type='dugg' uid='jobshopcc'/>
    <activity at='2008-07-01T07:19:45-04:00' guid='152481659' type='dugg' uid='rileynathanael'/>
</activities>");
        }

        public static Collection Collection()
        {
            return UTF8XmlSerializer.Deserialize<Collection>(
@"<collection name='example1'>
    <uid name='john.doe' publisher.name='twitter'/>
    <uid name='jane' publisher.name='digg'/>
</collection>");
        }
    }
}
