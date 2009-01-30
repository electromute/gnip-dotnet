using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
	
	/// <summary> 
    /// A model object that references a Gnip publisher.  A Gnip Publisher receives incoming {@link Activity} data
	/// and passes that data along in two forms:
	/// &lt;ol&gt;
	/// &lt;li&gt;as a stream of activity notifications&lt;/li&gt;
	/// &lt;li&gt;to {@link Filter} objects associated with the publisher which in turn make activity data available
	/// as either activities or notifications based on whether the {@link Filter} is configured as
	/// {@link Filter#setFullData(boolean)}.
	/// &lt;/li&gt;
	/// &lt;/ol&gt;
	/// 
	/// This model object is typically used to create a new Publisher or to get the details of an existing Publisher.
	/// </summary>
    [Serializable]
    [XmlRoot(ElementName="publisher")]
    public class Publisher : IResource, IDeepCompare
	{
        /// <summary>
        /// Constructor.
        /// </summary>
        public Publisher() 
        { 
             this.Type = PublisherType.Gnip;
             this.SupportedRuleTypes = new List<RuleType>();
        }

        /// <summary> 
        /// Basic constructor.
        /// </summary>
        /// <param name="type">The publisher type.</param>
		/// <param name="name">the name of the publisher</param>
		public Publisher(PublisherType type, string name) : this()
        {
            this.Type = type;
            this.Name = name;
        }

        /// <summary>
        /// Create a publisher. Note, calling this constructor does not 
        /// create the publisher on a Gnip server.
        /// </summary>
        /// <param name="type">The publisher type.</param>
        /// <param name="name">the publisher's name</param>
        /// <param name="ruleTypes">the publisher's rule types</param>
        public Publisher(PublisherType type, string name, IEnumerable<RuleType> ruleTypes)
            : this(type, name)
        {
            this.SupportedRuleTypes.AddRange(ruleTypes);
        }

        /// <summary>
        /// Create a publisher. Note, calling this constructor does not 
        /// create the publisher on a Gnip server.
        /// </summary>
        /// <param name="type">The publisher type.</param>
        /// <param name="name">the publisher's name</param>
        /// <param name="ruleTypes">the publisher's rule types</param>
        public Publisher(PublisherType type, string name, params RuleType[] ruleTypes) 
            : this(type, name, (IEnumerable<RuleType>)ruleTypes) { }

        /// <summary> 
        /// Gets/Sets the PublisherType of this publisher.
        /// </summary>
        /// <returns>the name of the publisher</returns>
        [XmlIgnore]
        public PublisherType Type { get; set; }

		/// <summary> 
        /// Retrieves the name of this publisher.
        /// </summary>
		/// <returns>the name of the publisher</returns>
        [XmlAttribute(AttributeName="name")]
        public string Name { get; set; }
		
        /// <summary>
        /// Gets/Sets the RuleTypes.
        /// </summary>
        [XmlArray(ElementName="supportedRuleTypes")]
        [XmlArrayItem(ElementName="type")]
        public List<RuleType> SupportedRuleTypes { get; set; }

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

            return this.DeepEquals((Publisher)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Publisher that)
        {
            if(this == that)
                return true;
            else if(that == null)
                return false;

            return (this.Type == that.Type &&
                string.Equals(this.Name, that.Name) &&
                ListUtils.AreEqualIgnoreOrder<RuleType>(this.SupportedRuleTypes, that.SupportedRuleTypes));
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

            Publisher that = (Publisher)o;

            return this.Type == that.Type && 
                String.Equals(this.Name, that.Name) &&
                this.SupportedRuleTypes == that.SupportedRuleTypes;
		}

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
		public override int GetHashCode()
		{
            int result = this.Type.GetHashCode();
            result = 31 * result + (this.Name != null ? this.Name.GetHashCode() : 0);
            result = 31 * result + (this.SupportedRuleTypes != null ? this.SupportedRuleTypes.GetHashCode() : 0);
            return result;
		}
	}
}