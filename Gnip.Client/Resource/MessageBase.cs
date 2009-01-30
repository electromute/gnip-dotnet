using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
    /// <summary> 
    /// Represents a message returned to a client from the Gnip server.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "messageBase")]
    public class MessageBase : IResource, IDeepCompare
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MessageBase() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageBase(string message)
            : this()
        {
            this.Message = message;
        }

        /// <summary> 
        /// Gets/Sets the messageBase message.
        /// </summary>
        [XmlText]
        public string Message { get; set; }

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

            return this.DeepEquals((MessageBase)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public virtual bool DeepEquals(MessageBase that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return (this.Message == that.Message);
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
            else if (o == null)
                return false;

            return this.DeepEquals((MessageBase)o);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            return (this.Message != null ? this.Message.GetHashCode() : 0);
        }
    }
}
