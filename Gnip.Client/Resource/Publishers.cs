using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
	/// <summary> 
    /// Container class that wraps a set of Publisher.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName="publishers")]
	public class Publishers : IResource, IDeepCompare
	{
        private List<Publisher> publishers = new List<Publisher>();

		/// <summary> 
        /// Basic constructor.
        /// </summary>
		public Publishers() { }

        /// <summary>
        /// Contructor. Adds a copy of publishers to this.
        /// </summary>
        /// <param name="publishers">The IEnumerable of Publishers to add to this.</param>
        public Publishers(IEnumerable<Publisher> publishers) 
        {
            this.publishers.AddRange(publishers);
        }

        /// <summary>
        /// Gets/Sets the List of Publisher.
        /// </summary>
        [XmlElement(ElementName="publisher")]
        public List<Publisher> Items
        {
            get { return this.publishers; }
            set { this.publishers = value; }
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

            return this.DeepEquals((Publishers)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Publishers that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return ListUtils.AreDeepEqual<Publisher>(this.publishers, that.publishers);
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

            Publishers that = (Publishers)o;

            return (this.publishers == that.publishers);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result = (this.publishers != null ? this.publishers.GetHashCode() : 0);
            return result;
        }
	}
}