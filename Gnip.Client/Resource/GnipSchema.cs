using System;
using System.Xml.Schema;
using System.IO;
using System.Reflection;

namespace Gnip.Client.Resource
{
    /// <summary> 
    /// A wrapper that provides access to Gnip's XSD document.
    /// </summary>
    public static class GnipSchema
    {
        private static XmlSchema singleton;

        public static XmlSchema Instance
        {
            get
            {
                if (GnipSchema.singleton == null)
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    using (Stream stream = assembly.GetManifestResourceStream("Gnip.Client.Xsd.gnip.xsd"))
                    {
                        GnipSchema.singleton = XmlSchema.Read(stream, null);
                    }
                }
                return GnipSchema.singleton;
            }
        }
    }
}