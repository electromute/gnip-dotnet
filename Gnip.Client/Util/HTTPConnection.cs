using System;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using log4net;
using Gnip.Client.Utils;
using Gnip.Client.Resource;
using System.Reflection;
using System.Globalization;

namespace Gnip.Client.Util
{

    /// <summary> 
    /// Basic abstraction atop an HTTP connection that is used to handle low-level Gnip <> HTTP protocol interaction.
    /// This class is used by the {@link com.Gnip.Client.GnipConnection} to communicate with a Gnip server
    /// and is not intended to be used by clients.
    /// 
    /// This class sets several headers on the request, in part based on the how the connection is configured
    /// by the {@link com.Gnip.Client.Config} instance.
    /// <ul>
    /// <li>
    /// <code>Content-Encoding</code> and <code>Accept-Encoding</code> are set if {@link Config#setUseGzip(boolean)} is
    /// <code>true</code>
    /// </li>
    /// <li><code>User-Agent</code> is set to a Java-client value that includes the version of the client library</li>
    /// <li><code>Authorization</code> is set using basic authentication credentials</li>
    /// </ul> 
    /// </summary>
    internal class HTTPConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HTTPConnection));
        private static readonly string USER_AGENT_STRING;

        private Config config;

        static HTTPConnection()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            AssemblyName name = thisAssembly.GetName();
            Version version = name.Version;
            USER_AGENT_STRING = "Gnip-Client-CSharp/" + version.Major + "." + version.Minor + "." + version.Build;
        }

        /// <summary> Create a new {@link HTTPConnection} with the provided configuration.</summary>
        /// <param name="config">the configuration for the connection
        /// </param>
        public HTTPConnection(Config config)
        {
            this.config = config;
        }

        /// <summary> 
        /// Get the Timespan different betweet the server time and the client time. This allows
        /// the server time to be approximated more accurately by
        /// DateTime.Now.Add(GetServerTimeDelta());
        /// </summary>
        /// <param name="urlString">the URL to receive the GET</param>
        /// <returns>the TimeSpan difference between this computer and the server.</returns>
        /// <throws>IOException if an exception occurs communicating with the server</throws>
        public TimeSpan GetServerTimeDelta()
        {
            Uri url = new Uri(this.config.GnipServer);
            HttpWebRequest urlConnection = (System.Net.HttpWebRequest)WebRequest.Create(url);
            urlConnection.Method = "GET";
            urlConnection.UserAgent = USER_AGENT_STRING;
            urlConnection.Timeout = config.RequestTimeout;
            urlConnection.ReadWriteTimeout = config.ReadWriteTimeout;

            DateTime now = DateTime.Now;
            WebHeaderCollection headers =  GetHeaders(urlConnection);
            String dateString = headers["Date"];

            TimeSpan ret = TimeSpan.Zero;

            if (dateString != null)
            {
                DateTime dateTime = DateTime.ParseExact(dateString, "R", null).ToLocalTime();
                ret = dateTime.Subtract(now);
            }

            return ret;
        }

        /// <summary> 
        /// Send an HTTP request of type GET to the given URL.
        /// </summary>
        /// <param name="urlString">the URL to receive the GET</param>
        /// <returns>the Stream from the response</returns>
        /// <throws>IOException if an exception occurs communicating with the server</throws>
        public Stream DoGet(string urlString)
        {
            HttpWebRequest urlConnection = CreateConnection(urlString, HTTPMethod.GET);
            Log.Debug("HTTP GET to " + urlString);
            return GetData(urlConnection);
        }

        /// <summary>
        /// Send an HTTP request of type POST to the given URL with the given data for the request body.
        /// </summary>
        /// <param name="urlString">the URL to receive the POST</param>
        /// <param name="data">the bytes to send in the request body</param>
        /// <returns>the Result from the response</returns>
        /// <throws>IOException if an exception occurs communicating with the server</throws>
        public Result DoPost(string urlString, byte[] data)
        {
            HttpWebRequest urlConnection = CreateConnection(urlString, HTTPMethod.POST);
            Log.Debug("HTTP POST to " + urlString);

            if (!config.UseGzip && Log.IsDebugEnabled)
                Log.Debug("with data " + (data == null ? "null" : StringUtils.ToString(data)));

            Result ret = TransferData(data, urlConnection);

            if (Log.IsDebugEnabled)
                Log.Debug("POST Result Message: " + ret.Message);

            return ret;
        }

        /// <summary> 
        /// Send an HTTP request of type PUT to the given URL with the given data for the request body.
        /// </summary>
        /// <param name="urlString">the URL to receive the PUT</param>
        /// <param name="data">the bytes to send in the request body</param>
        /// <returns>the Result from the response</returns>
        /// <throws> IOException if an exception occurs communicating with the server </throws>
        public Result DoPut(string urlString, byte[] data)
        {
            HttpWebRequest urlConnection = CreateConnection(urlString, HTTPMethod.PUT);

            Log.Debug("HTTP PUT to " + urlString);

            if (!config.UseGzip)
                Log.Debug("with data " + StringUtils.ToString(data));

            return TransferData(data, urlConnection);
        }

        /// <summary> Send an HTTP request of type DELETE to the given URL.</summary>
        /// <param name="urlString">the URL to receive the DELETE
        /// </param>
        /// <returns> the {@link InputStream} from the response
        /// </returns>
        /// <throws>  IOException if an exception occurs communicating with the server </throws>
        public virtual Result DoDelete(string urlString)
        {
            HttpWebRequest urlConnection = CreateConnection(urlString, HTTPMethod.DELETE);
            Log.Debug("HTTP DELETE to " + urlString);
            Stream resultStream = GetData(urlConnection);
            Result ret = XmlHelper.Instance.FromXmlStream<Result>(resultStream);
            return ret;
        }

        /// <summary>
        /// Sends data through the request via put, or post.
        /// </summary>
        /// <param name="data">The data to sent</param>
        /// <param name="request">The request</param>
        /// <returns>The Result from the response.</returns>
        private Result TransferData(byte[] data, HttpWebRequest request)
        {
            request.ContentLength = (data == null ? 0 : data.Length);

            Log.Debug("Starting data transfer at " + DateTime.Now);

            if (data != null)
            {
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                //dataStream.Close();
            }

            Log.Debug("Finished data transfer at " + DateTime.Now);
            Log.Debug("Awaiting server response...");

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex)
            {
                Log.Error("Exception Getting Response. Url: " + request.Address, wex);
                throw;
            }

            Log.Debug("Received response with response code " + response.StatusDescription);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                string errorMessage = "";
                try
                {
                    Stream errorStream = response.GetResponseStream();
                    Error error = XmlHelper.Instance.FromXmlStream<Error>(errorStream);
                    errorMessage = error.Message;
                    errorStream.Close();
                    response.Close();
                }
                catch (Exception e)
                {
                    Log.Debug("Exception occurred unmarshalling error message.", e);
                }
                throw new IOException("Error with request code: " + response.StatusCode + " message: " + response.StatusDescription + " " + errorMessage);
            }

            Log.Debug("Starting data read at " + System.DateTime.Now);

            Stream stream;

            string contentEncoding = response.Headers.Get("Content-Encoding");
            if (contentEncoding != null && contentEncoding.ToUpper(CultureInfo.InvariantCulture).Contains("GZIP"))
            {
                stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            else
            {
                stream = response.GetResponseStream();
            }

            Result ret = XmlHelper.Instance.FromXmlStream<Result>(stream);

            stream.Close();
            response.Close();

            return ret;
        }

        /// <summary>
        /// Gets data base on the request.
        /// </summary>
        /// <param name="urlConnection"></param>
        /// <returns></returns>
        private Stream GetData(HttpWebRequest request)
        {
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new IOException("Error with request code:" + response.StatusCode + " message: " + response.StatusDescription);
                }
            }
            catch (WebException wex)
            {
                Log.Error("Exception Getting Response. Url: " + request.Address, wex);
                throw;
            }

            Stream stream;

            string contentEncoding = response.Headers.Get("Content-Encoding");
            if (contentEncoding != null && contentEncoding.ToUpper().Contains("GZIP"))
            {
                stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            else
            {
                stream = response.GetResponseStream();
            }

            // pump from stream to result data.
            MemoryStream resultData = new MemoryStream();
            StreamUtils.Pump(stream, resultData);
            resultData.Position = 0;

            stream.Close();
            response.Close();
            resultData.Position = 0;

            if (Log.IsDebugEnabled)
                Log.Debug("GetData: data: " + StringUtils.ToString(resultData.ToArray()));

            return resultData;

        }

        // <summary>
        /// Gets the response headers for the request.
        /// </summary>
        /// <param name="urlConnection"></param>
        /// <returns>WebHeaderCollection</returns>
        private WebHeaderCollection GetHeaders(HttpWebRequest request)
        {
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new IOException("Error with request code:" + response.StatusCode + " message: " + response.StatusDescription);
                }
            }
            catch (WebException wex)
            {
                Log.Error("Exception Getting Response. Url: " + request.Address, wex);
                throw;
            }

            WebHeaderCollection collection = response.Headers;

            response.Close();

            return collection;
        }

        /// <summary>
        /// Creates a new HttpWebRequest based on the urlString, method and config.
        /// </summary>
        /// <param name="urlString">The urlString</param>
        /// <param name="method">The connection method</param>
        /// <returns>A new HttpWebRequest.</returns>
        private HttpWebRequest CreateConnection(string urlString, HTTPMethod method)
        {
            Uri url = new Uri(urlString);
            HttpWebRequest urlConnection = (System.Net.HttpWebRequest)WebRequest.Create(url);
            urlConnection.Method = method.ToString();
            urlConnection.ContentType = "application/xml";
            urlConnection.Credentials = new NetworkCredential(config.Username, config.Password);
            urlConnection.UserAgent = USER_AGENT_STRING;
            urlConnection.Timeout = config.RequestTimeout;
            urlConnection.ReadWriteTimeout = config.ReadWriteTimeout;

            if (config.UseGzip)
            {
                urlConnection.Headers.Add("Accept-Encoding", "gzip");
                urlConnection.Headers.Add("Content-Encoding", "gzip");
            }

            return urlConnection;
        }
    }
}