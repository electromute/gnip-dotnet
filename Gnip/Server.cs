using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Gnip
{
    public interface IServer
    {
        string Get(string url);
        string Post(string url, string xmlContent);
        string Put(string url, string xmlContent);
        string Delete(string url);
    }

    public class Server : IServer
    {
        public string Username;
        public string Password;

        public string BaseUrl = "https://review.gnipcentral.com";

        public Server(string username, string password) 
        {
            this.Username = username;
            this.Password = password;
        }

        public string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(BaseUrl + url);
            request.Credentials = new NetworkCredential(Username, Password);
            
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)  
            {
                if (response.StatusCode != HttpStatusCode.OK) throw new Exception(string.Format("Server returned {0}\n {1}", response.StatusCode, response.ToString()));
                // Cheat and always expect utf-8
                return new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8).ReadToEnd();
            }
        }

        public string Post(string url, string xmlContent)
        {
            return Do("POST", url, xmlContent);
        }

        public string Put(string url, string xmlContent)
        {
            return Do("PUT", url, xmlContent);
        }

        public string Delete(string url)
        {
            return Do("DELETE", url, null);
        }

        private string Do(string verb, string url, string xmlContent)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUrl + url);
            request.Credentials = new NetworkCredential(Username, Password);
            request.Method = verb;

            if (xmlContent != null)
            {
                request.ContentType = "application/xml";
                byte[] byteData = Encoding.UTF8.GetBytes(xmlContent);
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            {
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }
    }
}
