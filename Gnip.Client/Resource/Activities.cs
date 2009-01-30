using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
	
	/// <summary> Container class that wraps a set of {@link Activity} instances that will be sent to a Gnip server
	/// or were read from a Gnip server.
	/// </summary>
    [Serializable]
    [XmlRoot(ElementName="activities")]
	public class Activities : IResource, IDeepCompare
	{
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Activities() 
        {
            this.Items = new List<Activity>();
        }

        /// <summary> 
        /// Constructor.
        /// </summary>
        /// <param name="rules">Adds the rules to this list.</param>
        public Activities(IEnumerable<Activity> activities)
            : this()
        {
            this.Items.AddRange(activities);
        }

		/// <summary> 
        /// If these activities came from a Publisher, retrieves the url of the publisher
        /// </summary>
		/// <returns> the name of the Publisher or <code>null</code> if the activities were created in the client.
		/// </returns>
        [XmlAttribute(AttributeName = "publisher")]
        public string Publisher { get; set; }

        /// <summary>
        /// Gets/Sets the List of Activitie.
        /// </summary>
        [XmlElement(ElementName = "activity")]
        public List<Activity> Items { get; set; }

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

            return this.DeepEquals((Activities)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Activities that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return (this.Publisher == that.Publisher &&
                ListUtils.AreDeepEqual<Activity>(this.Items, that.Items));
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

            Activities that = (Activities)o;

            return (this.Publisher == that.Publisher &&
                this.Items == that.Items);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result = (this.Publisher != null ? this.Publisher.GetHashCode() : 0);
            result = 31 * result + (this.Items != null ? this.Items.GetHashCode() : 0);
            return result;
        }
	}
}