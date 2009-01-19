using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
	
	/// <summary> 
    /// Container class that wraps a set of Rules.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "rules")]
	public class Rules : IResource, IDeepCompare
	{
        private List<Rule> rules = new List<Rule>();

		/// <summary> 
        /// Default constructor.
        /// </summary>
        public Rules() { }

        /// <summary> 
        /// Constructor.
        /// </summary>
        /// <param name="rules">Adds the rules to this list.</param>
        public Rules(IEnumerable<Rule> rules) 
        {
            this.rules.AddRange(rules);
        }

        /// <summary>
        /// Gets/Sets the List of Rule.
        /// </summary>
        [XmlElement(ElementName = "rule")]
        public List<Rule> Items
        {
            get { return this.rules; }
            set { this.rules = value; }
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

            return this.DeepEquals((Rules)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Rules that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return ListUtils.AreDeepEqual<Rule>(this.rules, that.rules);
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

            Rules that = (Rules)o;

            return (this.rules == that.rules);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result = (this.rules != null ? this.rules.GetHashCode() : 0);
            return result;
        }
	}
}