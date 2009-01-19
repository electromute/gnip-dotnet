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
        private string publisher;
        private List<Activity> activities = new List<Activity>();

		/// <summary> 
        /// If these activities came from a Publisher, retrieves the url of the publisher
        /// </summary>
		/// <returns> the name of the Publisher or <code>null</code> if the activities were created in the client.
		/// </returns>
        [XmlAttribute(AttributeName="publisher")]
		public string Publisher
		{
			get { return this.publisher; }
            set { this.publisher = value; }
		}

        /// <summary>
        /// Gets/Sets the List of Activitie.
        /// </summary>
        [XmlElement(ElementName = "activity")]
        public List<Activity> Items
        {
            get { return this.activities; }
            set { this.activities = value; }
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

            return (this.publisher == that.publisher &&
                ListUtils.AreDeepEqual<Activity>(this.activities, that.activities));
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

            return (this.publisher == that.publisher &&
                this.activities == that.activities);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result = (this.publisher != null ? this.publisher.GetHashCode() : 0);
            result = 31 * result + (this.activities != null ? this.activities.GetHashCode() : 0);
            return result;
        }
	}
}