using System;
using System.Xml.Serialization;
using Gnip.Client.Util;
using System.Collections.Generic;
using System.Collections;

namespace Gnip.Client.Resource
{
	
	/// <summary>
    /// Model object that represents a Gnip activity.  An activity is approximately equivalent to an event
	/// that occurs on a Publisher; for example, on Twitter a tweet is an activity and digging an article is an activity
	/// on Digg.
    ///
	/// An Activity may represent a simple "notification" of an event that can be read from a Publisher's notification
	/// stream or from the notification stream for a {@link Filter} that does not support full data.
    ///
	/// An Activity may also represent full activity data for an event and can be read from a {@link Filter} that
	/// is configured to support full data.
	/// </summary>
    [Serializable]
    [XmlRoot(ElementName = "activity")]
	public class Activity : IResource, IDeepCompare
	{
        private DateTime at;
        private string action;
        private string activityId;
        private string url;
        private List<string> sources;
        private List<string> keywords;
        private List<Place> places;
        private List<Actor> actors;
        private List<GnipUrl> destinationUrls;
        private List<GnipValue> tags;
        private List<GnipValue> tos;
        private List<GnipUrl> regardingUrls;
        private Payload payload;

        /// <summary> 
        /// A basic constructor for creating an activity. This constructor does not initilize any of the Lists
        /// and is setup to use primarity for the serializer.
        /// </summary>
        public Activity()
        {
        }

        /// <summary> 
        /// A basic constructor for creating an activity.
        /// </summary>
        /// <param name="init">when set to true, all Lists are initialized with new List&lt;/T&gt;()</param>
        public Activity(bool init)
        {
            if (init) this.Initialize();
        }

        /// <summary> A basic constructor for creating an activity from an actor, often a username, and an
        /// action, often something the actor did.
        /// </summary>
        /// <param name="at">the time at which the activity occured.</param>
        /// <param name="action">the action performed by the actor
        /// </param>
        public Activity(DateTime at, string action)
            : this(true)
        {
            this.at = at;
            this.action = action;
        }

        /// <summary> A basic constructor for creating an activity from an actor, often a username, and an
        /// action, often something the actor did.
        /// </summary>
        /// <param name="at">the time at which the activity occured.</param>
        /// <param name="action">the action performed by the actor</param>
        /// <param name="payload">the data associated with the activity</param>
        public Activity(DateTime at, string action, Payload payload)
            : this(at, action)
        {
            this.payload = payload;
        }

        /// <summary> A basic constructor for creating an activity from an actor, often a username, and an
        /// action, often something the actor did.
        /// </summary>
        /// <param name="actor">the actor</param>
        /// <param name="action">the action performed by the actor
        /// </param>
        public Activity(Actor actor, string action)
            : this(DateTime.Now, action)
        {
            this.actors.Add(actor);
        }

        /// <summary> A constructor for creating an activity from an actor, often a username, and an action, often
        /// something the actor did.  This constructor also includes a Payload of data that is
        /// associated with the activity.
        /// </summary>
        /// <param name="actor">the actor</param>
        /// <param name="action">the action performed by the actor</param>
        /// <param name="payload">the data associated with the activity</param>
        public Activity(Actor actor, string action, Payload payload)
            : this(actor, action)
        {
            this.payload = payload;
        }

        /// <summary>
        /// Initialize all Lists with new List&lt;/T&gt;()
        /// </summary>
        protected virtual void Initialize()
        {
            this.sources = new List<string>();
            this.keywords = new List<string>();
            this.places = new List<Place>();
            this.actors = new List<Actor>();
            this.destinationUrls = new List<GnipUrl>();
            this.tags = new List<GnipValue>();
            this.tos = new List<GnipValue>();
            this.regardingUrls = new List<GnipUrl>();
        }

        /// <summary> 
        /// Gets/Sets the caller-provided time that represents when the activity occurred.
        /// </summary>
		/// <returns>the time</returns>
        [XmlElement(ElementName="at")]
        public System.DateTime At {
            get { return this.at; }
            set { this.at = value; }
        }

        /// <summary>
        /// Gets/Sets the action performed by the actor.
        /// </summary>
        [XmlElement(ElementName = "action")]
        public string Action
        {
            get { return this.action; }
            set { this.action = value; }
        }

        /// <summary>
        /// Gets/Sets the action performed by the activityId.
        /// </summary>
        [XmlElement(ElementName = "activityID")]
        public string ActivityId
        {
            get { return this.activityId; }
            set { this.activityId = value; }
        }

        /// <summary>
        /// Gets/Sets the URL associated with the action.
        /// </summary>
        [XmlElement(ElementName = "URL")]
        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        /// <summary>
        /// Gets/Sets the sources of the activity.
        /// </summary>
        [XmlElement(ElementName = "source")]
        public List<string> Sources
        {
            get { return this.sources; }
            set { this.sources = value; }
        }

        /// <summary>
        /// Gets/Sets the keywords of the activity.
        /// </summary>
        [XmlElement(ElementName = "keyword")]
        public List<string> Keywords
        {
            get { return this.keywords; }
            set { this.keywords = value; }
        }

        /// <summary>
        /// Gets/Sets the places of the activity.
        /// </summary>
        [XmlElement(ElementName = "place")]
        public List<Place> Places
        {
            get { return this.places; }
            set { this.places = value; }
        }


        /// <summary>
        /// Gets/Sets the Actor name.
        /// </summary>
        [XmlElement(ElementName = "actor")]
        public List<Actor> Actors
        {
            get { return this.actors; }
            set { this.actors = value; }
        }

        /// <summary>
        /// Gets/Sets to the links of the activity.
        /// </summary>
        [XmlElement(ElementName = "destinationURL")]
        public List<GnipUrl> DestinationUrls
        {
            get { return this.destinationUrls; }
            set { this.destinationUrls = value; }
        }

        /// <summary>
        /// Gets/Sets any tags associated with the activity; for example, tags set on a photo.
        /// </summary>
        [XmlElement(ElementName = "tag")]
        public List<GnipValue> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        /// <summary>
        /// Gets/Sets to whom or to what the activity refers.
        /// </summary>
        [XmlElement(ElementName = "to")]
        public List<GnipValue> Tos 
        {
            get { return this.tos; }
            set { this.tos = value; }
        }
        
        /// <summary>
        /// Gets/Sets what the activity regards.
        /// </summary>
        [XmlElement(ElementName = "regardingURL")]
        public List<GnipUrl> RegardingUrls
        {
            get { return this.regardingUrls; }
            set { this.regardingUrls = value; }
        }

        /// <summary>
        /// Gets/Sets the data associated with the activity
        /// </summary>
        [XmlElement(ElementName = "payload")]
        public Payload Payload
        {
            get { return this.payload; }
            set { this.payload = value; }
        }
		
		/// <summary>
        /// Retrieves the value from the Activity that is associated with the RuleType.
        /// </summary>
		/// <param name="ruleType">the rule type</param>
		/// <returns>the value associated with the rule type</returns>
        public virtual IList getValue(RuleType ruleType)
        {
            IList ret = null;

            switch (ruleType)
            {
                case RuleType.Actor:
                    ret = this.Actors;
                    break;
                case RuleType.Tag:
                    ret = this.Tags;
                    break;
                case RuleType.To:
                    ret = this.Tos;
                    break;
                case RuleType.Regarding:
                    ret = this.RegardingUrls;
                    break;
                case RuleType.Source:
                    ret = this.Sources;
                    break;
            }

            return ret;
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

            return this.DeepEquals((Activity)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Activity that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return (this.at == that.at &&
                string.Equals(this.action, that.action) &&
                string.Equals(this.activityId, that.activityId) &&
                string.Equals(this.url, that.url) &&
                ListUtils.AreEqual<string>(this.sources,that.sources) &&
                ListUtils.AreEqual<string>(this.keywords, that.keywords) &&
                ListUtils.AreDeepEqual<Place>(this.places,that.places) &&
                ListUtils.AreDeepEqual<Actor>(this.actors,that.actors) &&
                ListUtils.AreDeepEqual<GnipUrl>(this.destinationUrls,that.destinationUrls) &&
                ListUtils.AreDeepEqual<GnipValue>(this.tags,that.tags) &&
                ListUtils.AreDeepEqual<GnipValue>(this.tos,that.tos) &&
                ListUtils.AreDeepEqual<GnipUrl>(this.regardingUrls,that.regardingUrls) &&
                ObjectUtils.AreDeepEqual(this.payload,that.payload));
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
			
			Activity that = (Activity) o;

            return (this.at == that.at &&
                string.Equals(this.action, that.action) &&
                string.Equals(this.activityId, that.activityId) &&
                string.Equals(this.url, that.url) &&
                this.sources == that.sources &&
                this.keywords == that.keywords &&
                this.places == that.places &&
                this.actors == that.actors &&
                this.destinationUrls == that.destinationUrls &&
                this.tags == that.tags &&
                this.tos == that.tos &&
                this.regardingUrls == that.regardingUrls &&
                this.payload == that.payload);
		}

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
		public override int GetHashCode()
		{
			int result = at.GetHashCode();
            result = 31 * result + (this.action != null ? this.action.GetHashCode() : 0);
            result = 31 * result + (this.activityId != null ? this.activityId.GetHashCode() : 0);
            result = 31 * result + (this.url != null ? url.GetHashCode() : 0);
			return result;
		}
	}
}