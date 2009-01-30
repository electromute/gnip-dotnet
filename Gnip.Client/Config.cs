using System;
namespace Gnip.Client
{
	
	/// <summary> 
    /// Configuration information for a {@link GnipConnection}.
	/// It supports basic settings
	/// for username, password, and URL of the Gnip service to connect to as
	/// well as advanced settings including an option to tunnel PUT and DELETE
	/// requests over POST and configuring network timeouts.
	/// 
    /// The default connection URL is: https://prod.gnipcentral.com
	/// </summary>
	public class Config
	{
        public const string DEFAULT_SERVER_URL = "https://prod.gnipcentral.com";
        public const int DEFAULT_REQUEST_TIMEOUT_SECONDS = 30;
        public const int DEFAULT_READ_TIMEOUT_SECONDS = 10;

        /// <summary> 
        /// Create a Config} object with the specified username and password
        /// to the default Gnip server.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public Config(string username, string password)
        {
            this.Username = username;
            this.Password = password;
            this.GnipServer = DEFAULT_SERVER_URL;
            this.RequestTimeout = DEFAULT_REQUEST_TIMEOUT_SECONDS * 1000;
            this.ReadWriteTimeout = DEFAULT_READ_TIMEOUT_SECONDS * 1000;
        }

        /// <summary> Create a Config object to the Gnip server at the provided URL
        /// with the username and password credentials.
        /// </summary>
        /// 
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="gnipServer">The gnip server to use.</param>
        public Config(string username, string password, Uri gnipServer)
        {
            this.Username = username;
            this.Password = password;
            this.GnipServer = gnipServer.ToString();
            this.RequestTimeout = DEFAULT_REQUEST_TIMEOUT_SECONDS * 1000;
            this.ReadWriteTimeout = DEFAULT_READ_TIMEOUT_SECONDS * 1000;
        }

        /// <summary> Create a Config object to the Gnip server at the provided URL
        /// with the username and password credentials.
        /// </summary>
        /// 
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="gnipServer">The gnip server to use.</param>
        /// <param name="requestTimeout">The timeout used for obtaining an http request.</param>
        /// <param name="readWriteTimeout">The timeout used for reading and writing to a request.</param>
        public Config(string username, string password, Uri gnipServer, int requestTimeout, int readWriteTimeout)
        {
            this.Username = username;
            this.Password = password;
            this.GnipServer = gnipServer.ToString();
            this.RequestTimeout = requestTimeout;
            this.ReadWriteTimeout = readWriteTimeout;
        }

		/// <summary> 
        /// Retrieves the username used to connect to Gnip.
        /// </summary>
        public string Username { get; set; }

		/// <summary> Gets/Sets the password used to connect to Gnip.</summary>
        public string Password { get; set; }

		/// <summary> 
        /// Gets/Sets the URL of the Gnip service.
        /// </summary>
        public string GnipServer { get; set; }

        /// <summary> Gets/Sets Configure the setting that controls how long the local HTTP connection will
        /// wait for a Gnip server's request for a response or a response stream.  When making requests that
        /// transmit large amounts of data, including creating Filters with large
        /// rule sets, or that are sent over slow networks, the connection will be more
        /// reliable when this timeout is increased. This value is configured in milliseconds.<br/>
        /// 
        /// The default value of this setting is 2 seconds<br/>
        /// </summary>
        public int RequestTimeout { get; set; }

		/// <summary> Gets/Sets Configure the setting that controls how long the local HTTP connection will
		/// wait for a read or write to the Gnip server.  When making requests that
		/// transmit large amounts of data, including creating Filters with large
		/// rule sets, or that are sent over slow networks, the connection will be more
		/// reliable when this timeout is increased. This value is configured in milliseconds.<br/>
		/// 
		/// The default value of this setting is 2 seconds<br/>
		/// </summary>
        public int ReadWriteTimeout { get; set; }

		/// <summary> 
        /// Gets/Sets the tunnel over post flag.  The default value is false.
        /// The TunnelOverPost Property controls whether the HTTP PUT and DELETE are
		/// sent to the server by tunneling them through an HTTP POST.  This setting
		/// can be useful when a network configuration disallows directly sending PUT
		/// and DELETE requests.  The default for this setting is <code>false</code>.
		/// </summary>
        public bool TunnelOverPost { get; set; }

		/// <summary> 
        /// Gets/Sets the compression flag.  The default value is false.
        /// The UseGzip Property controls whether HTTP requests and responses use
		/// the the gzip encoding which is set using the Content-Encoding
		/// and the Accept-Encoding HTTP headers.
		/// </summary>
        public bool UseGzip { get; set; }
	}
}