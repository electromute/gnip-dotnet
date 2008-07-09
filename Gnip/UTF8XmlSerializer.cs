using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Gnip
{
    public class UTF8XmlSerializer
    {
        public static string Serialize<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            using (XmlTextWriter writer = new XmlTextWriter(stream, new UTF8Encoding(false)))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;

                new XmlSerializer(typeof(T)).Serialize(writer, obj);
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T) serializer.Deserialize(new StringReader(xml));
        }
    }
}