using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
    [Serializable]
    [XmlRoot(ElementName = "actor")]
    public class Actor : GnipValue, IDeepCompare
    {
        /// <summary>
        /// Default Contructor
        /// </summary>
        public Actor() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">the value of the actor</param>
        public Actor(string value) : this(value, null, null) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">the value for the Actor</param>
        /// <param name="uid">the Actor's uid</param>
        public Actor(string value, string uid) : this(value, uid, null) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="uid">the uid for the Actor</param>
        /// <param name="value">the value for the Actor</param>
        /// <param name="metaUrl">the meta url for the Actor</param>
        public Actor(string value, string uid, string metaUrl)
            : base(value, metaUrl)
        {
            this.Uid = uid;
        }

        /// <remarks/>
        [XmlAttribute(AttributeName = "uid")]
        public string Uid { get; set; }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member lists and objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public override bool DeepEquals(object o)
        {
            if (this == o)
                return true;
            if (o == null || GetType() != o.GetType())
                return false;

            return this.DeepEquals((Actor)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Actor that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return (base.DeepEquals(that) &&
                string.Equals(this.Uid, that.Uid));
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

            Actor that = (Actor)o;

            return this.DeepEquals(that);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = 31 * result + (this.Uid != null ? this.Uid.GetHashCode() : 0);
            return result;
        }
    }
}
