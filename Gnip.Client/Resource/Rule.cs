using System;
using System.Xml.Serialization;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
	
	/// <summary> A model object that represents a Gnip Rule.  A set of rules are set on a {@link Filter} and are used to match
	/// {@link Activity} objects that flow through a {@link Publisher publishers}.
	/// </summary>
    [Serializable]
    [XmlRoot(ElementName = "rule")]
    public class Rule : IResource, IDeepCompare
	{
        private RuleType type;
        private string value;

        public Rule() { }

        /// <summary>
        /// A simple constructor.
        /// </summary>
		/// <param name="type">the rule type</param>
		/// <param name="value">the rule's value</param>
		public Rule(RuleType type, string value)
		{
			this.type = type;
			this.value = value;
		}

        /// <summary> 
        /// Retrieves the rule's type.
        /// </summary>
        [XmlAttribute(AttributeName="type")]
        public RuleType Type {
            get { return this.type; }
            set { this.type = value; }
        }
        
        /// <summary> 
        /// Retrieves the rule's value.
        /// </summary>
        [XmlText]
        public string Value {
            get { return this.value; }
            set { this.value = value; }
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

            return this.DeepEquals((Rule)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Rule that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            if (string.Equals(this.type, that.type) &&
                string.Equals(this.value, that.value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified Object is equal to the current Object. Ths performs
        /// a shallow equals where any reference types are compared by reference.
        /// </summary>
        /// <param name="o">the specifies object</param>
        /// <returns>true if equal, false otherwise</returns>
		public override bool Equals(object o)
		{
			if (o == null || GetType() != o.GetType())
				return false;
			
			return this.DeepEquals((Rule) o);
		}

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
		public override int GetHashCode()
		{
			int result;
			result = type.GetHashCode();
			result = 31 * result + (value  != null ? value.GetHashCode() : 0);
			return result;
		}
	}
}