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
        /// <summary> 
        /// A basic constructor for creating an activity. 
        /// </summary>
        public Activity()
        {
            this.Sources = new List<string>();
            this.Keywords = new List<string>();
            this.Places = new List<Place>();
            this.Actors = new List<Actor>();
            this.DestinationUrls = new List<GnipUrl>();
            this.Tags = new List<GnipValue>();
            this.Tos = new List<GnipValue>();
            this.RegardingUrls = new List<GnipUrl>();
        }

        /// <summary> A basic constructor for creating an activity from an actor, often a username, and an
        /// action, often something the actor did.
        /// </summary>
        /// <param name="at">the time at which the activity occured.</param>
        /// <param name="action">the action performed by the actor
        /// </param>
        public Activity(DateTime at, string action)
            : this()
        {
            this.At = at;
            this.Action = action;
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
            this.Payload = payload;
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
            this.Actors.Add(actor);
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
            this.Payload = payload;
        }

        /// <summary> 
        /// Gets/Sets the caller-provided time that represents when the activity occurred.
        /// </summary>
        /// <returns>the time</returns>
        [XmlElement(ElementName = "at")]
        public System.DateTime At { get; set; }

        /// <summary>
        /// Gets/Sets the action performed by the actor.
        /// </summary>
        [XmlElement(ElementName = "action")]
        public string Action { get; set; }

        /// <summary>
        /// Gets/Sets the action performed by the activityId.
        /// </summary>
        [XmlElement(ElementName = "activityID")]
        public string ActivityId { get; set; }

        /// <summary>
        /// Gets/Sets the URL associated with the action.
        /// </summary>
        [XmlElement(ElementName = "URL")]
        public string Url { get; set; }

        /// <summary>
        /// Gets/Sets the sources of the activity.
        /// </summary>
        [XmlElement(ElementName = "source")]
        public List<string> Sources { get; set; }

        /// <summary>
        /// Gets/Sets the keywords of the activity.
        /// </summary>
        [XmlElement(ElementName = "keyword")]
        public List<string> Keywords { get; set; }

        /// <summary>
        /// Gets/Sets the places of the activity.
        /// </summary>
        [XmlElement(ElementName = "place")]
        public List<Place> Places { get; set; }


        /// <summary>
        /// Gets/Sets the Actor name.
        /// </summary>
        [XmlElement(ElementName = "actor")]
        public List<Actor> Actors { get; set; }

        /// <summary>
        /// Gets/Sets to the links of the activity.
        /// </summary>
        [XmlElement(ElementName = "destinationURL")]
        public List<GnipUrl> DestinationUrls { get; set; }

        /// <summary>
        /// Gets/Sets any tags associated with the activity; for example, tags set on a photo.
        /// </summary>
        [XmlElement(ElementName = "tag")]
        public List<GnipValue> Tags { get; set; }
        /// <summary>
        /// Gets/Sets to whom or to what the activity refers.
        /// </summary>
        [XmlElement(ElementName = "to")]
        public List<GnipValue> Tos { get; set; }

        /// <summary>
        /// Gets/Sets what the activity regards.
        /// </summary>
        [XmlElement(ElementName = "regardingURL")]
        public List<GnipUrl> RegardingUrls { get; set; }

        /// <summary>
        /// Gets/Sets the data associated with the activity
        /// </summary>
        [XmlElement(ElementName = "payload")]
        public Payload Payload { get; set; }

        /// <summary>
        /// Retrieves the value from the Activity that is associated with the RuleType.
        /// </summary>
        /// <param name="ruleType">the rule type</param>
        /// <returns>the value associated with the rule type</returns>
        public virtual IList GetValue(RuleType ruleType)
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

            return (this.At == that.At &&
                string.Equals(this.Action, that.Action) &&
                string.Equals(this.ActivityId, that.ActivityId) &&
                string.Equals(this.Url, that.Url) &&
                ListUtils.AreEqual<string>(this.Sources, that.Sources) &&
                ListUtils.AreEqual<string>(this.Keywords, that.Keywords) &&
                ListUtils.AreDeepEqual<Place>(this.Places, that.Places) &&
                ListUtils.AreDeepEqual<Actor>(this.Actors, that.Actors) &&
                ListUtils.AreDeepEqual<GnipUrl>(this.DestinationUrls, that.DestinationUrls) &&
                ListUtils.AreDeepEqual<GnipValue>(this.Tags, that.Tags) &&
                ListUtils.AreDeepEqual<GnipValue>(this.Tos, that.Tos) &&
                ListUtils.AreDeepEqual<GnipUrl>(this.RegardingUrls, that.RegardingUrls) &&
                ObjectUtils.AreDeepEqual(this.Payload, that.Payload));
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

            Activity that = (Activity)o;

            return (this.At == that.At &&
                string.Equals(this.Action, that.Action) &&
                string.Equals(this.ActivityId, that.ActivityId) &&
                string.Equals(this.Url, that.Url) &&
                this.Sources == that.Sources &&
                this.Keywords == that.Keywords &&
                this.Places == that.Places &&
                this.Actors == that.Actors &&
                this.DestinationUrls == that.DestinationUrls &&
                this.Tags == that.Tags &&
                this.Tos == that.Tos &&
                this.RegardingUrls == that.RegardingUrls &&
                this.Payload == that.Payload);
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result = At.GetHashCode();
            result = 31 * result + (this.Action != null ? this.Action.GetHashCode() : 0);
            result = 31 * result + (this.ActivityId != null ? this.ActivityId.GetHashCode() : 0);
            result = 31 * result + (this.Url != null ? Url.GetHashCode() : 0);
            return result;
        }
    }
}