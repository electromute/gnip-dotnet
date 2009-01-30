using System;
using System.Xml.Serialization;
using System.IO.Compression;
using System.Text;
using System.IO;
using Gnip.Client.Utils;
using System.Collections.Generic;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
	
	/// <summary> 
    /// Model object that represents the Payload of a Gnip com.Gnip.Client.Resource.Activity}.  For
	/// activities that support full-data, as opposed to just simple notifications, the {@link Payload} contains
	/// the raw activity data that was originally sent to the Publisher}.
	/// 
	/// Typically, a Gnip user would create a payload object in order to publish data into Gnip and would receive a
	/// payload object from an Activity} retrieved via a com.Gnip.Client.GnipConnection}.
	/// 
	/// A payload's body is expected to be formatted as a String.  A payload's raw can be either a normal or a
	/// Base64 encoded String in order to allow the caller to control the raw encoding.  When this object is serialized
	/// into XML, the raw String will <i>always</i> be Base64 encoded.
	/// </summary>
    [Serializable]
    [XmlRoot(ElementName = "payload")]
    public class Payload : IResource, IDeepCompare
	{
		private string raw;
        private string decodedRaw;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Payload() 
        {
            this.MediaUrls = new List<MediaUrl>();
        }

        /// <summary> 
        /// Create a basic payload object.  Typically, this method would be called when creating an {@link Activity}
        /// to publish into Gnip.  The <code>raw</code> passed here should <i>not</i> be Base64 encoded; if already
        /// encoded, it will be re-encoded.  To pass an encoded <code>raw</code> use the {@link #Payload(String, String, boolean)}
        /// constructor. 
        /// </summary>
        /// <param name="title">the title of the playload</param>
        /// <param name="body">the body of the activity</param>
        /// <param name="raw">an un-encoded representation of the activity payload</param>
        public Payload(string title, string body, string raw)
            : this(title, body, raw, false)
        {
        }

        /// <summary> Create a payload object.  This constructor can be used to create a Payload with an already encoded raw or</summary>
        /// <param name="title">the title of the playload</param>
        /// <param name="body">the value of the body</param>
        /// <param name="raw">the value of the activity's raw data</param>
        /// <param name="isEncoded">a flag set for whether the raw data is encoded</param>
        public Payload(string title, string body, string raw, bool isEncoded) 
            : this(title, body, null, raw, isEncoded)
        {
            this.Title = title;
            this.Body = body;
            this.Raw = isEncoded ? raw : encode(raw);
        }

        /// <summary> Create a payload object.  This constructor can be used to create a Payload with an already encoded raw or</summary>
        /// <param name="title">the title of the playload</param>
        /// <param name="body">the value of the body</param>
        /// <param name="raw">the value of the activity's raw data</param>
        /// <param name="isEncoded">a flag set for whether the raw data is encoded</param>
        public Payload(string title, string body, List<MediaUrl> mediaUrls, string raw, bool isEncoded)
            : this()
        {
            this.Title = title;
            this.Body = body;
            if (mediaUrls != null) this.MediaUrls.AddRange(mediaUrls);
            this.raw = isEncoded ? raw : encode(raw);
        }

        /// <summary> 
        /// Gets/Sets the payload's title.
        /// </summary>
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

		/// <summary> 
        /// Gets/Set the payload's body.
        /// </summary>
        [XmlElement(ElementName="body")]
        public string Body { get; set; }

        /// <summary> 
        /// Gets/Sets the payload's mediaUrls.
        /// </summary>
        [XmlElement(ElementName = "mediaURL")]
        public List<MediaUrl> MediaUrls { get; set; }

		/// <summary> 
        /// Gets/Sets the payload's raw data. This value will be Base64 encoded.</summary>
		/// <returns> the raw, encoded string</returns>
        [XmlElement(ElementName = "raw")]
		public string Raw
		{
			get { return this.raw; }
            set { this.raw = value; this.decodedRaw = null; }
		}

		/// <summary>Retrieves the payload's raw value after being Base64 decoded.</summary>
        [XmlIgnore]
	    public string DecodedRaw
		{
			get { return decodedRaw != null ? decodedRaw : (decodedRaw = decode(raw)); }
		}
		
        /// <summary>
        /// Encodes the string value by compressing it and then base 64 encoding it.
        /// </summary>
        /// <param name="value">the string to encode.</param>
        /// <returns>the encoded string.</returns>
		private string encode(string value)
		{
            string ret = string.Empty;

            if (value != null)
            {
                MemoryStream stream = new MemoryStream();
                using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    byte[] bytes = StringUtils.ToByteArray(value);
                    zipStream.Write(bytes, 0, bytes.Length);
                }

                ret = Convert.ToBase64String(stream.ToArray());
            }

            return ret;
		}

        /// <summary>
        /// Decodes a zipped Base64 encoded string.
        /// </summary>
        /// <param name="value">the value to decode</param>
        /// <returns>the decoded string</returns>
        private string decode(string value)
        {
            MemoryStream outputStream = new MemoryStream();
            MemoryStream inputStream = new MemoryStream(Convert.FromBase64String(value));

            byte[] buffer = new byte[1024];

            using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            {
                int bytesRead = 0;
                do
                {
                    bytesRead = zipStream.Read(buffer, 0, 1024);
                    outputStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);
            }

            string ret = StringUtils.ToString(outputStream.ToArray());
            outputStream.Close();

            return ret;
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member lists and objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(object o)
        {
            if (this == o)
                return true;
            if (o == null || GetType() != o.GetType())
                return false;

            return this.DeepEquals((Payload)o);
        }

        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Payload that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return (string.Equals(this.Title, that.Title) &&
                string.Equals(this.Body, that.Body) &&
                ListUtils.AreDeepEqual<MediaUrl>(this.MediaUrls, that.MediaUrls) &&
                string.Equals(this.raw, that.raw));
        }

        /// <summary>
        /// Determins if this equals that by performing a shallow equals.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (o == null || GetType() != o.GetType())
                return false;

            Payload that = (Payload)o;

            return (string.Equals(this.Title, that.Title) &&
                string.Equals(this.Body, that.Body) &&
                string.Equals(this.MediaUrls, that.MediaUrls) &&
                string.Equals(this.raw, that.raw));
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result = (this.Title != null ? this.Title.GetHashCode() : 0);
            result = 31 * result + (this.Body != null ? this.Body.GetHashCode() : 0);
            result = 31 * result + (this.raw != null ? this.raw.GetHashCode() : 0);
            return result;
        }
	}
}