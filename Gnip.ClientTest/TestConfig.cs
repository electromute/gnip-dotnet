using System;
using System.Collections.Specialized;
using Gnip.Client.Resource;
namespace Gnip.Client
{
    public class TestConfig
	{
        private static TestConfig singleton = null;

		public static TestConfig Instance
		{
			get
			{
                if (TestConfig.singleton != null)
                    return TestConfig.singleton;

                NameValueCollection appSettings = System.Configuration.ConfigurationManager.AppSettings;
                TestConfig.singleton = new TestConfig(
                    appSettings["gnip.username"], 
                    appSettings["gnip.password"], 
                    appSettings["gnip.host"],
                    (PublisherType)Enum.Parse(typeof(PublisherType), appSettings["gnip.publisherType"], true), 
                    appSettings["gnip.publisher"],
                    int.Parse(appSettings["gnip.requestTimeout"]),
                    int.Parse(appSettings["gnip.readWriteTimeout"]),
                    int.Parse(appSettings["gnip.idlemillis"]));

                return TestConfig.singleton;
			}
		}

        private TestConfig(string username, string password, string host, PublisherType publisherType, string publisher, int requestTimeout, int readWriteTimeout, int idleMilliseconds)
		{
			this.Username = username;
			this.Password = password;
			this.Host = host;
            this.PublisherType = publisherType;
			this.Publisher = publisher;
            this.RequestTimeout = requestTimeout;
            this.ReadWriteTimeout = readWriteTimeout;
            this.IdleMilliseconds = idleMilliseconds;
		}

        public string Username { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public PublisherType PublisherType { get; set; }

        public string Publisher { get; set; }

        public int RequestTimeout { get; set; }

        public int ReadWriteTimeout { get; set; }

        public int IdleMilliseconds { get; set; }
	}
}