﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
    [Serializable]
    [XmlRoot(ElementName = "gnipValue")]
    public class GnipValue : IResource, IDeepCompare
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public GnipValue() { }

        /// <summary>
        /// Constructor with Url value.
        /// </summary>
        /// <param name="value">The value</param>
        public GnipValue(string value)
            : this()
        {
            this.Value = value;
        }

        /// <summary>
        /// Constructor with metaUrl value.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="value">The metaURL</param>
        public GnipValue(string value, string metaUrl)
            : this(value)
        {
            this.MetaUrl = metaUrl;
        }

        /// <summary>
        /// Gets/Sets the value.
        /// </summary>
        [XmlText]
        public string Value { get; set; }


        /// <summary>
        /// Gets/Sets the MetaUrl value
        /// </summary>
        [XmlAttribute(AttributeName = "metaURL", DataType = "anyURI")]
        public string MetaUrl { get; set; }

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

            return this.DeepEquals((GnipValue)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(GnipValue that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return (string.Equals(this.MetaUrl, that.MetaUrl) &&
                string.Equals(this.Value, that.Value));
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

            return this.DeepEquals((GnipValue)o);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result = (this.Value != null ? this.Value.GetHashCode() : 0);
            result = 31 * result + (this.MetaUrl != null ? this.MetaUrl.GetHashCode() : 0);
            return result;
        }
    }
}
