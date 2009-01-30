using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
    [Serializable]
    [XmlRoot(ElementName = "mediaURL")]
    public class MediaUrl : IResource, IDeepCompare
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public MediaUrl() { }

        /// <summary>
        /// Constructor with Url value.
        /// </summary>
        /// <param name="url">The url</param>
        public MediaUrl(string url)
            : this()
        {
            this.Url = url;
        }

        /// <summary>
        /// Constructor with all values.
        /// </summary>
        /// <param name="url">The url value</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="duration">The duration</param>
        /// <param name="mimeType">The mimeType</param>
        /// <param name="type">The type</param>
        public MediaUrl(string url, string width, string height, string duration, string mimeType, string type)
            : this(url)
        {
            this.Width = width;
            this.Height = height;
            this.Duration = duration;
            this.MimeType = mimeType;
            this.Type = type;
        }

        /// <summary>
        /// Gets/Sets the url value.
        /// </summary>
        [XmlText]
        public string Url { get; set; }

        /// <summary>
        /// Gets/Sets the height value
        /// </summary>
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }

        /// <summary>
        /// Gets/Sets the width value
        /// </summary>
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }

        /// <summary>
        /// Gets/Sets the duration value
        /// </summary>
        [XmlAttribute(AttributeName = "duration")]
        public string Duration { get; set; }

        /// <summary>
        /// Gets/Sets the mimeType value
        /// </summary>
        [XmlAttribute(AttributeName = "mimeType")]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets/Sets the type value
        /// </summary>
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member lists and objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public virtual bool DeepEquals(object o)
        {
            if (this == o)
                return true;
            if (o == null || GetType() != o.GetType())
                return false;

            return this.DeepEquals((MediaUrl)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(MediaUrl that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return (string.Equals(this.Url, that.Url) &&
                string.Equals(this.Height, that.Height) &&
                string.Equals(this.Width, that.Width) &&
                string.Equals(this.Duration, that.Duration) &&
                string.Equals(this.MimeType, that.MimeType) &&
                string.Equals(this.Type, that.Type));
        }

        /// <summary>
        /// Determines whether the specified Object is equal to the current Object. Ths performs
        /// a shallow equals where any reference types are compared by reference.
        /// </summary>
        /// <param name="o">the specifies object</param>
        /// <returns>true if equal, false otherwise</returns>
        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (o == null || GetType() != o.GetType())
                return false;

            return this.DeepEquals((MediaUrl)o);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result = (this.Url != null ? this.Url.GetHashCode() : 0);
            result = 31 * result + (this.Height != null ? this.Height.GetHashCode() : 0);
            result = 31 * result + (this.Width != null ? this.Width.GetHashCode() : 0);
            result = 31 * result + (this.Duration != null ? this.Duration.GetHashCode() : 0);
            result = 31 * result + (this.MimeType != null ? this.MimeType.GetHashCode() : 0);
            result = 31 * result + (this.Type != null ? this.Type.GetHashCode() : 0);
            return result;
        }
    }
}
