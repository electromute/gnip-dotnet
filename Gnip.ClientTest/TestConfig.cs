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

                NameValueCollection appSettings = System.Configuration.ConfigurationSettings.AppSettings;
                TestConfig.singleton = new TestConfig(
                    appSettings["gnip.username"], 
                    appSettings["gnip.password"], 
                    appSettings["gnip.host"],
                    (PublisherType)Enum.Parse(typeof(PublisherType), appSettings["gnip.publisherType"], true), 
                    appSettings["gnip.publisher"],
                    int.Parse(appSettings["gnip.idlemillis"]));

                return TestConfig.singleton;
			}
		}

        private string username;
        private string password;
        private string host;
        private PublisherType publisherType;
        private string publisher;
        private int idleMilliseconds;

        private TestConfig(string username, string password, string host, PublisherType publisherType, string publisher, int idleMilliseconds)
		{
			this.username = username;
			this.password = password;
			this.host = host;
            this.publisherType = publisherType;
			this.publisher = publisher;
            this.idleMilliseconds = idleMilliseconds;
		}

		public string Username
		{
			get { return this.username; }
            set { this.username = value; }
		}

		public string Password
		{
			get { return this.password; }
            set { this.password = value; }
		}

		public string Host
		{
			get { return this.host; }
            set { this.host = value; }
		}

        public PublisherType PublisherType
        {
            get { return this.publisherType; }
            set { this.publisherType = value; }
        }

		public string Publisher
		{
            get { return this.publisher; }
            set { this.publisher = value; }
		}

		public int IdleMilliseconds
		{
			get { return this.idleMilliseconds; }
            set { this.idleMilliseconds = value; }
		}
	}
}