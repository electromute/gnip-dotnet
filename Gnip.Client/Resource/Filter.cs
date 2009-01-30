using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{

    /// <summary> A model object that represents a Gnip filter.  A Filter is a stream containing activities from a Publisher
    /// that meet subscriber defined criteria.  For example,
    /// a Filter would allow a subscriber to create activity streams that contain just the activities from
    /// usernames (actors) in which they are interested.  These criteria are defined by adding Rule rules to
    /// a Filter via {@link #addRule(Rule)}.
    /// 
    /// A Filter can support either full-data activities or notifications but not both by setting the Filter's
    /// IsFullData full data flag.  By default, a Filter supports full data.
    /// 
    /// By default, a Filter creates an activity stream that is available at an HTTP endpoint on a Gnip server
    /// and can be accessed (even polled) using HTTP GET.  If a Filter specifies a post URL, then the Gnip
    /// server will push activities from the Filter's activity stream to the post URL using an HTTP POST
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "filter")]
    public class Filter : IResource, IDeepCompare
    {
        /// <summary> 
        /// Create a Filter.
        /// </summary>
        public Filter()
        {
            this.Rules = new List<Rule>();
        }


        /// <summary> 
        /// Create a Filter with the given name.
        /// </summary>
        /// <param name="name">the filter's name</param>
        public Filter(string name) : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Create a Filter with the given name and post URL.
        /// </summary>
        /// <param name="name">the filter name</param>
        /// <param name="postUrl">the post URL</param>
        public Filter(string name, string postUrl)
            : this(name)
        {
            this.PostUrl = postUrl;
        }

        /// <summary> 
        /// Create a Filter with the given name, post url, and full data flag.  If the Filter supports full-data
        /// activities, the {@link #fullData} should be set to true.  If set to false,
        /// the Filter will just support notifications.
        /// </summary>
        /// <param name="name">the filter name</param>
        /// <param name="postUrl">the post URL</param>
        /// <param name="isFullData">does the filter support the full data</param>
        public Filter(string name, string postUrl, bool isFullData)
            : this(name, postUrl)
        {
            this.IsFullData = isFullData;
        }

        /// <summary>
        /// Create a Filter with the given name, post url, and full data flag.  If the Filter supports full-data
        /// activities, the {@link #fullData} should be set to true.  If set to false,
        /// the Filter will just support notifications.
        /// </summary>
        /// <param name="name">the filter name</param>
        /// <param name="postUrl">the post URL</param>
        /// <param name="isFullData">does the filter support the full data</param>
        /// <param name="rules">The rules to add to this filter</param>
        public Filter(string name, string postUrl, bool isFullData, IEnumerable<Rule> rules)
            : this(name, postUrl, isFullData)
        {
            this.Rules.AddRange(rules);
        }

        /// <summary>
        /// Create a Filter with the given name, post url, and full data flag.  If the Filter supports full-data
        /// activities, the {@link #fullData} should be set to true.  If set to false,
        /// the Filter will just support notifications.
        /// </summary>
        /// <param name="name">the filter name</param>
        /// <param name="postUrl">the post URL</param>
        /// <param name="isFullData">does the filter support the full data</param>
        /// <param name="rules">The rules to add to this filter.</param>
        public Filter(string name, string postUrl, bool isFullData, params Rule[] rules) 
            : this(name, postUrl, isFullData, (IEnumerable<Rule>)rules) { }

        /// <summary> 
        /// Gets/Sets the filter's post URL. If set, the post URL is used by a Gnip server to send either
        /// full data activites or just activity notifications to the subscriber via an HTTP POST.
        /// </summary>
        [XmlElement("postURL")]
        public string PostUrl { get; set; }

        /// <summary> 
        /// Get the list of rules associated with this filter.  {@link Rule Rules} are subscriber-specified
        /// criteria used to activities from a Publisher to the Filter's activity stream. 
        /// </summary>
        [XmlElement(ElementName="rule")]
        public List<Rule> Rules { get; set; }

        /// <summary>
        /// Retrieves the name of this Filter.
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        /// <summary> 
        /// Gets/Sets the flag describing whether this filter supports access to full activity data or
        /// just notifications. To add full activity data to the Filter's stream, set this value to <code>true</code>;
        /// otherwise to only add notifications to the stream, set the flag to <code>false</code>.
        /// </summary>
        /// <returns> true if the filter supports full data; false otherwise if
        /// the filter just supports notifications.</returns>
        [XmlAttribute(AttributeName = "fullData")]
        public bool IsFullData { get; set; }

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

            return this.DeepEquals((Filter)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Filter that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return 
                (string.Equals(this.PostUrl, that.PostUrl) &&
                string.Equals(this.Name, that.Name) &&
                this.IsFullData == that.IsFullData &&
                ListUtils.AreDeepEqual<Rule>(this.Rules, that.Rules));
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

            Filter that = (Filter)o;

            return
                (string.Equals(this.PostUrl, that.PostUrl) &&
                string.Equals(this.Name, that.Name) &&
                this.IsFullData == that.IsFullData &&
                this.Rules == that.Rules);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result;
            result = (this.Rules != null ? this.Rules.GetHashCode() : 0);
            result = 31 * result + (this.Name != null ? this.Name.GetHashCode() : 0);
            result = 31 * result + (this.PostUrl != null ? this.PostUrl.GetHashCode() : 0);
            result = 31 * result + this.IsFullData.GetHashCode();
            return result;
        }
    }
}