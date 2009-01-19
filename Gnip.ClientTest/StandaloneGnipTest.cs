using System;
using Gnip.Client.Resource;
using System.Collections.Generic;
namespace Gnip.Client
{

    /// <summary>
    /// A simple, standalone class with a <code>main</code> method that can be used to
    /// test gnip.
    /// </summary>
    public class StandaloneGnipTest
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Config config = new Config("<username>", "<password>");
            GnipConnection gnip = new GnipConnection(config);
            Publisher publisher = gnip.GetPublisher(PublisherType.Gnip, "gnip-test-publisher");
        }
    }
}