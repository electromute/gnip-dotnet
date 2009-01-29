using System;
using System.IO;
using Gnip.Client.Utils;
using Gnip.Client.Resource;
using log4net;
using Gnip.Client.Util;
using System.Web;
using System.Text;
using System.IO.Compression;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace Gnip.Client
{

    /// <summary> 
    /// Represents a client connection to a Gnip service.  This class encapsulates all protocol
    /// level interactions with a Gnip service and provides a higher-level abstraction for writing
    /// data to and reading data from Gnip.
    /// 
    /// Users interested in consuming data from Gnip will be specifically interested in reading
    /// notifications from a Publisher and/or either notifications or activities from a Filter using:
    /// 
    /// <ul>
    /// <li>GetNotifications(Gnip.Client.Resource.Publisher) or GetNotifications(Gnip.Client.Resource.Publisher, DateTime)
    /// to read notifications from a Publisher
    /// </li>
    /// <li>Create(Gnip.Client.Resource.Publisher, Gnip.Client.Resource.Filter) to create a Filter</li>
    /// <li>GetActivities(Gnip.Client.Resource.Publisher, Gnip.Client.Resource.Filter) or 
    /// GetActivities(Gnip.Client.Resource.Publisher, Gnip.Client.Resource.Filter, DateTime)
    /// to read activities from a Filter (or notifications if the Filter doesn't support full-data.
    /// </li> 
    /// <br/>
    /// <br/>
    /// Users interested in publishing data into Gnip will be specifically interested in creating Publishers
    /// and sending activities to publishers using:
    /// <ul>
    /// <li>
    /// Greate(Gnip.Client.Resource.Publisher) to create a Publisher.
    /// </li>
    /// <li>
    /// Publish(Gnip.Client.Resource.Publisher, Gnip.Client.Resource.Activities) to publish
    /// activities into a Publisher
    /// </li>
    /// </ul>
    /// </summary>
    public class GnipConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GnipConnection));

        private const long BUCKET_SIZE = 60 * 1000;

        private HTTPConnection connection;
        private Config config;
        private TimeSpan timeCorrection = TimeSpan.Zero;

        /// <summary>
        /// Create a new GnipConnection with the specified config.
        /// </summary>
        /// <param name="config">The config used to init the connection.</param>
        public GnipConnection(Config config)
        {
            this.connection = new HTTPConnection(config);
            this.config = config;
        }

        /// <summary> 
        /// Gets/Sets the Gnip.Client.Config configuration used to establish and
        /// authenticate a Gnip connection.
        /// </summary>
        public Config Config
        {
            get { return this.config; }
            set { this.config = value; }
        }

        /// <summary>
        /// Gets/Sets a TimeSpan that is added to the DateTime passed to 
        /// GetActivities(..., DateTime) abd GetNotifications(..., DateTime). Typically this is 
        /// either set to TimeSpan.Zero (default) or GetServerTimeDelta(). 
        /// 
        /// When activities are published to date buckets, they are published accoring to
        /// the gnip server GMT time. Thus, when passing a client generated dateTime as a parameter to
        /// the methods mentioned above, you may not get expected results if your client time is 
        /// different than that of the server, which it likely is. For instance, say you want all the
        /// activities published one minute ago. you would get the current time and subtract one minute.
        /// However, that time is likely to be, at the very least, a little different than the server 
        /// time. You have two options to adjust that time. you can add the results of GetServerTimeDelta()
        /// to the local time, or you can set TimeCorrection to GetServerTimeDelta() and the GnipConnection
        /// will automatically add that TimeSpan to the the dateTime passed to the GetActivities and 
        /// GetNotifications methods.
        /// </summary>
        public TimeSpan TimeCorrection
        {
            get { return this.timeCorrection; }
            set { this.timeCorrection = value; }
        }

        /// <summary>
        /// This method gets a TimeSpan that is the difference between the client time and the server time. 
        /// Adding this timespan to the local machine time should approximate the servers
        /// actual time. This value can then used to adjust times when getting time sensitive data such as
        /// getting activities from buckets. Please see the TimeCorrection property for more information.
        /// </summary>
        public TimeSpan GetServerTimeDelta()
        {
            try
            {
                return this.connection.GetServerTimeDelta();
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred synching time", e);
            }
            catch(FormatException e)
            {
                throw new GnipException("Exception occurred synching time", e);
            }
        }

        /// <summary> 
        /// Retrieves the list of Publishers avaialble from Gnip.
        /// </summary>
        /// <returns>the list of Gnip.Client.Resource.Publishers</returns>
        /// <throws>GnipException if there were problems authenticating with the Gnip server 
        /// or if another error occurred.</throws>
        public Publishers GetPublishers(PublisherType type)
        {
            try
            {
                string pulisherUrl = this.GetPublishersUrl(type) + ".xml";
                Stream response = connection.DoGet(pulisherUrl);
                Log.Debug("GetPublisher: url: " + pulisherUrl + " response: " + response);
                Publishers publishers = XmlHelper.Instance.FromXmlStream<Publishers>(response);

                if (publishers != null)
                {
                    foreach (Publisher publisher in publishers.Items)
                    {
                        publisher.Type = type;
                    }
                }
                return publishers;
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred getting Publishers", e);
            }
        }

        /// <summary>
        /// Create a new Publisher on the Gnip server.
        /// </summary>
        /// <param name="publisher">The Publisher to post to the Gnip server.</param>
        /// <returns>The Result</returns>
        /// <throws>GnipException if the Publisher already exists, if there were problems authenticating with a Gnip 
        /// server, or if another error occurred.</throws>
        public Result Create(Publisher publisher)
        {
            try
            {
                byte[] data = this.ConvertToBytes<Publisher>(publisher);
                return connection.DoPost(this.GetPublishersUrl(publisher.Type), data);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred creating Publisher", e);
            }
        }

        /// <summary>
        /// Create a Filter on a Publisher on the Gnip server.
        /// </summary>
        /// <param name="publisher">the publisher that owns the filter</param>
        /// <param name="filter">the filter to create</param>
        /// <throws>GnipException if the Filter already exists, if there were problems authenticating with a Gnip
        /// server, or if another error occurred.</throws>
        public Result Create(Publisher publisher, Filter filter)
        {
            try
            {
                byte[] data = this.ConvertToBytes<Filter>(filter);
                return connection.DoPost(this.GetFilterCreateUrl(publisher.Type, publisher.Name), data);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred creating Filter", e);
            }
        }

        /// <summary>
        /// Gets the Publisher with name = publisherName from the gnip server.
        /// </summary>
        /// <param name="type">The PublisherType</param>
        /// <param name="publisherName">name of the publisher to get</param>
        /// <returns> the Publisher if it exists</returns>
        /// <throws>GnipException if the publisher doesn't exist, if there were problems authentiating with the Gnip server,
        /// or if another error occurred.</throws>                    
        public Publisher GetPublisher(PublisherType type, string publisherName)
        {
            try
            {
                Stream response = connection.DoGet(this.GetPublishersUrl(type, publisherName));
                Publisher publisher = XmlHelper.Instance.FromXmlStream<Publisher>(response);

                if (publisher != null)
                {
                    publisher.Type = type;
                }

                return publisher;
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred getting Publisher", e);
            }
        }

        /// <summary> Retrieves the Filter named Gnip.Client.Resource.Filter.Name from the 
        /// Publisher named Gnip.Client.Resource.Publisher.Name
        /// </summary>
        /// <param name="publisher">the publisher that owns the filter</param>
        /// <param name="filter">the filter to retrieve</param>
        /// <returns>the Filter if it exists</returns>
        /// <throws>GnipException if the Filter doesn't exist, if there were problems authenticating with the Gnip server,
        /// or if another error occurred.</throws>
        public Filter GetFilter(Publisher publisher, Filter filter)
        {
            if (publisher == null)
            {
                throw new ArgumentException("Publisher cannot be null");
            }

            if (filter == null)
            {
                throw new ArgumentException("Filter cannot be null");
            }

            return this.GetFilter(publisher.Type, publisher.Name, filter.Name);
        }

        /// <summary>
        /// Retrieves the Filter named filterName from the 
        /// Publisher named Gnip.Client.Resource.Publisher.Name
        /// </summary>
        /// <param name="publisher">the publisher that owns the filter</param>
        /// <param name="filterName">the name of the filter to retrieve</param>
        /// <returns>the Filter if it exists</returns>
        /// <throws>GnipException if the Filter doesn't exist, if there were problems authenticating with the Gnip server,
        /// or if another error occurred.</throws>
        public Filter GetFilter(Publisher publisher, string filterName)
        {
            if (publisher == null)
            {
                throw new ArgumentException("Publisher cannot be null");
            }

            if (filterName == null)
            {
                throw new ArgumentException("Filter name cannot be null");
            }

            return this.GetFilter(publisher.Type, publisher.Name, filterName);
        }

        /// <summary>
        /// Gets the Filer from the gnip server with publisher name = publisherName and
        /// filter name = filterName.
        /// </summary>
        /// <param name="type">The PublisherType</param>
        /// <param name="publisherName">the name of the publisher</param>
        /// <param name="filterName">the filter to retrieve</param>
        /// <returns>the Filter} if it exists</returns>
        /// <throws>GnipException if the Filter doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Filter GetFilter(PublisherType type, string publisherName, string filterName)
        {
            try
            {
                Stream response = connection.DoGet(this.GetFilterUrl(type, publisherName, filterName));
                Log.Debug("GetFilter: publisherName: " + publisherName + " filterName: " + filterName + " response: " + response);
                return XmlHelper.Instance.FromXmlStream<Filter>(response);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred getting Filter", e);
            }
        }

        /// <summary>
        /// Update a Publisher on the Gnip server.
        /// </summary>
        /// <param name="publisher">The Publisher to update.</param>
        /// <returns>The Result</returns>
        /// <throws>GnipException if the Publisher does not already exists, if there were problems authenticating with a Gnip 
        /// server, or if another error occurred.</throws>
        public Result Update(Publisher publisher)
        {
            try
            {
                byte[] data = this.ConvertToBytes(publisher);
                Result result = null;

                if (config.TunnelOverPost)
                {
                    result = connection.DoPost(this.TunnelEditOverPost(this.GetPublisherUrl(publisher.Type, publisher.Name)), data);
                }
                else
                {
                    result = connection.DoPut(this.GetPublisherUrl(publisher.Type, publisher.Name), data);
                }

                return result;
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred updating Filter", e);
            }
        }

        /// <summary> 
        /// Update the Filter associated with the Publisher. As practiced in the
        /// REST style, this call updates the represetntation of the given Filter 
        /// by replacing the one that already exists. It does not do a merge of the two 
        /// Filter documents.
        /// 
        /// To do incremental updates of a Filter, use
        /// Update(com.Gnip.Client.Resource.Publisher, com.Gnip.Client.Resource.Filter, com.Gnip.Client.Resource.Rule)} or
        /// Update(com.Gnip.Client.Resource.Publisher, com.Gnip.Client.Resource.Filter, com.Gnip.Client.Resource.Rules)} to
        /// add rules to an existing Filter. 
        /// </summary>
        /// 
        /// <param name="publisher">the publisher that owns the filter</param>
        /// <param name="filter">the filter to update</param>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Update(Publisher publisher, Filter filter)
        {
            try
            {
                byte[] data = this.ConvertToBytes(filter);
                Result result = null;

                if (config.TunnelOverPost)
                {
                    result = connection.DoPost(this.TunnelEditOverPost(this.GetFilterUrl(publisher.Type, publisher.Name, filter.Name)), data);
                }
                else
                {
                    result = connection.DoPut(this.GetFilterUrl(publisher.Type, publisher.Name, filter.Name), data);
                }

                return result;
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred updating Filter", e);
            }
        }

        /// <summary> Update the Filter by adding a single rule to it. </summary>
        /// <param name="publisher">the publisher that owns the filter</param>
        /// <param name="filter">the filter to update</param>
        /// <param name="rule">the rule to add to the filter</param>
        /// <returns>The Result of the Update</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Update(Publisher publisher, Filter filter, Rule rule)
        {
            return this.Update(publisher, filter.Name, rule);
        }

        /// <summary> Update the Filter by adding a single rule to it. </summary>
        /// <param name="publisher">the publisher that owns the filter</param>
        /// <param name="filterName">the filter to update</param>
        /// <param name="rule">the rule to add to the filter</param>
        /// <returns>The Result of the Update</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Update(Publisher publisher, string filterName, Rule rule)
        {
            try
            {
                byte[] data = this.ConvertToBytes(rule);
                return connection.DoPost(this.GetRulesUrl(publisher.Type, publisher.Name, filterName), data);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred updating Rule", e);
            }
        }

        /// <summary>
        /// Update the Filter by bulk adding a Rules document containing many Rules.
        /// </summary>
        /// <param name="publisher">the publisher that owns the filter</param>
        /// <param name="filter">the filter to update]</param>
        /// <param name="rules">the set of rules to add to the filter</param>
        /// <returns>The Result of the Update</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Update(Publisher publisher, Filter filter, Rules rules)
        {
            return this.Update(publisher, filter.Name, rules);
        }

        /// <summary>
        /// Update the Filter by bulk adding a Rules document containing many Rules.
        /// </summary>
        /// <param name="publisher">the publisher that owns the filter]</param>
        /// <param name="filterName">the filter to update]</param>
        /// <param name="rules">the set of rules to add to the filter]</param>
        /// <returns>The Result of the Update</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Update(Publisher publisher, string filterName, Rules rules)
        {
            try
            {
                byte[] data = this.ConvertToBytes(rules);
                return connection.DoPost(this.GetRulesUrl(publisher.Type, publisher.Name, filterName), data);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred updating Rule", e);
            }
        }

        /// <summary> Delete the Filter from the Publisher.</summary>
        /// <param name="publisher">the publisher from which to delete the filter]</param>
        /// <param name="filter">the filter to delete]</param>
        /// <returns>The Result of the Update</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Delete(Publisher publisher, Filter filter)
        {
            return this.Delete(publisher, filter.Name);
        }

        /// <summary> Delete the Filter from the Publisher.</summary>
        /// <param name="publisher">the publisher from which to delete the filter</param>
        /// <param name="filterName">the name of the filter to delete</param>
        /// <returns>The Result of the Update</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Delete(Publisher publisher, string filterName)
        {
            try
            {
                Result ret = null;

                if (config.TunnelOverPost)
                {
                    ret = connection.DoPost(this.TunnelDeleteOverPost(this.GetFilterUrl(publisher.Type, publisher.Name, filterName)), new byte[0]);
                }
                else
                {
                    ret = connection.DoDelete(this.GetFilterUrl(publisher.Type, publisher.Name, filterName));
                }

                return ret;
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred deleting Filter", e);
            }
        }

        /// <summary> Delete the rule from the Filter associated with the Publisher.</summary>
        /// <param name="publisher">the publisher from which to delete a filter's rule</param>
        /// <param name="filter">the filter from which to remove a rule]</param>
        /// <param name="rule">the rule to remove</param>
        /// <returns>The Result of the Update</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Delete(Publisher publisher, Filter filter, Rule rule)
        {
            return this.Delete(publisher, filter.Name, rule);
        }

        /// <summary> Delete the rule from the Filter associated with the Publisher.</summary>
        /// <param name="publisher">the publisher from which to delete a filter's rule</param>
        /// <param name="filterName">the name of the filter from which to remove a rule</param>
        /// <param name="rule">the rule to remove</param>
        /// <returns>The Result of the Update</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Delete(Publisher publisher, string filterName, Rule rule)
        {
            try
            {
                Result ret = null;
                string url = this.GetRulesDeleteUrl(publisher, filterName, rule);

                if (config.TunnelOverPost)
                {
                    ret = connection.DoPost(url, null);
                }
                else
                {
                    ret = connection.DoDelete(url);
                }

                return ret;
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred deleting Rule", e);
            }
        }

        /// <summary>
        /// Publish Activity data in an Activities model to a Publisher. In order to publish
        /// activities, the credentials set in the Config instance associated with this GnipConnection
        /// must own the publisher.
        /// </summary>
        /// <param name="publisher">the publisher to publish activities to</param>
        /// <param name="activities">the activities to publish</param>
        /// <returns>Result, null of there were no activities to publish, otherwise the results of the publish.</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Result Publish(Publisher publisher, Activities activities)
        {
            if (activities == null || activities.Items.Count == 0)
                return null;

            try
            {
                byte[] data = this.ConvertToBytes<Activities>(activities);
                return connection.DoPost(this.GetActivitiesPublishUrl(publisher), data);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred publishing activities", e);
            }
        }

        /// <summary> 
        /// Retrieves the Activity data from the given Publisher for the current
        /// activity bucket.  This method expects that the publisher has a public timeline and makes 
        /// full activity data available to the credentials set in the Config instance used to 
        /// configure this GnipConnection. Note, not all Publisher publishers have a timeline 
        /// of activity data; for an up-to-date list of publishers that make such data available, 
        /// check the &lt;a href="https://prod.gnipcentral.com"&gt; Gnip Developer&lt;/a&gt; website.  
        /// Additionally, not all publishers provide access to complete
        /// activity data and instead typically just provide access to notifications.
        /// 
        /// Most Gnip users will need to use GetNotifications(Gnip.Client.Resource.Publisher)
        /// or GetNotifications(Gnip.Client.Resource.Publisher, DateTime) to
        /// get the notifications for a Publisher.
        /// </summary>
        /// <param name="publisher">the publisher whose activities to get</param>
        /// <returns> the Activities model, which contains a set of Activity activities.</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Activities GetActivities(Publisher publisher)
        {
            try
            {
                Stream inputStream = connection.DoGet(this.GetActivityUrl(publisher, false, DateTime.MinValue));
                return XmlHelper.Instance.FromXmlStream<Activities>(inputStream);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred getting activities", e);
            }
        }

        /// <summary> 
        /// Retrieves the Activity data from the given Publisher for an activity bucket at a given date.
        /// This method expects that publisher the has a public timeline and that the publisher makes
        /// full activity data available to the credentials set in the Config instance used to configure
        /// this GnipConnection.
        /// 
        /// Note, not all Publisher publishers have a timeline of activity data.
        /// 
        /// For an up-to-date list of publishers that have a public timeline, see the
        /// &lt;a href="https://prod.gnipcentral.com"&gt; Gnip Developer&lt;/a&gt; website.
        /// 
        /// Additionally, not all publishers provide access to complete
        /// activity data and instead typically just provide access to notifications.
        /// 
        /// Most Gnip users will need to use GetNotifications(Gnip.Client.Resource.Publisher)
        /// or GetNotifications(Gnip.Client.Resource.Publisher, System.DateTime) to
        /// get the notifications for a Publisher.
        /// 
        /// </summary>
        /// <param name="publisher">the publisher whose activities to get</param>
        /// <param name="dateTime">the timestamp of the activity bucket to retrieve </param>
        /// <returns> the Activities model, which contains a set of Activity activities or an empty
        /// Activities object if no notifications where found in the activity bucket</returns>
        /// <param name="dateTime">timestamp - DateTime.MinValue insures that activities are
        /// read from the latest bucket. Refer to the TimeCorrection property for more 
        /// information about the dateTime parameter.</param>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Activities GetActivities(Publisher publisher, DateTime dateTime)
        {
            try
            {
                Stream inputStream = connection.DoGet(this.GetActivityUrl(publisher, false, dateTime));
                return XmlHelper.Instance.FromXmlStream<Activities>(inputStream);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred getting activities", e);
            }
        }

        /// <summary> Retrieves either the notifications or activities from the current bucket for the given Publisher
        /// and Filter based on whether the filter supports full data.  See the Filter class for more information
        /// about whether a Filter supports notifications or activities.Items.  If the Filter supports notifications, the
        /// Activities object returned here will just have activity notifications.
        /// </summary>
        /// <param name="publisher">the publisher that owns the filter</param>
        /// <param name="filter">the filter whose notifications or activities to retrieve</param>
        /// <returns> the notifications or activities in the current bucket or an empty Activities object if no
        /// notifications or activities were found in the current bucket.
        /// </returns>
        /// <throws>GnipException if the publisher or filter don't exist, if there were problems 
        /// authenticating with the Gnip server, or if another error occurred.</throws>           
        public Activities GetActivities(Publisher publisher, Filter filter)
        {
            try
            {
                Stream inputStream = connection.DoGet(this.GetActivityUrl(publisher, filter, DateTime.MinValue));
                return XmlHelper.Instance.FromXmlStream<Activities>(inputStream);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred getting activities for Filter", e);
            }
        }

        /// <summary> 
        /// Retrieves either the notifications or activities from the bucket with the given timestamp for the given Publisher
        /// and Filter} based on whether the filter supports full data. See the Filter class for more information
        /// about whether a Filter supports notifications or activities.Items. If the Filter supports notifications, the
        /// Activities} object returned here will just have activity notifications.
        /// </summary>
        /// <param name="publisher">the publisher that owns the filter</param>
        /// <param name="filter">the filter whose notifications or activities to retrieve</param>
        /// <param name="dateTime">timestamp - DateTime.MinValue insures that activities are
        /// read from the latest bucket. Refer to the TimeCorrection property for more 
        /// information about the dateTime parameter.</param>
        /// <returns> the notifications or activities in the current bucket or an empty Activities object if no
        /// notifications or activities were found in the current bucket.
        /// </returns>
        /// <throws>GnipException if the publisher or filter don't exist, if there were problems 
        /// authenticating with the Gnip server, or if another error occurred.</throws>
        public Activities GetActivities(Publisher publisher, Filter filter, DateTime dateTime)
        {
            try
            {
                Stream inputStream = connection.DoGet(this.GetActivityUrl(publisher, filter, dateTime));
                return XmlHelper.Instance.FromXmlStream<Activities>(inputStream);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred getting activities for Filter", e);
            }
        }

        /// <summary> Retrieves the Activity notifications from the given Publisher for the current
        /// notification bucket.  This method can be invoked against all publishers that have a public timeline.
        /// For an up-to-date list of publishers that have a public timeline, see the
        /// &lt;a href="https://prod.gnipcentral.com"&t; Gnip Developer%lt;/a&gt; website.
        /// 
        /// Remember, notifications are just that -- notifications that an activity occurred on a Publisher.
        /// A notification does not contain an activity's complete data.  To obtain full activity data,
        /// use a Filter that has Filter.IsFullData = true}.
        /// </summary>
        /// <param name="publisher">the publisher whose notifications to get</param>
        /// <returns>the Activities model, which contains a set of Activity objects, or an empty
        /// Activities object if no notifications were found in the current notification bucket.</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Activities GetNotifications(Publisher publisher)
        {
            try
            {
                Stream inputStream = connection.DoGet(this.GetActivityUrl(publisher, true, DateTime.MinValue));
                return XmlHelper.Instance.FromXmlStream<Activities>(inputStream);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred getting activities", e);
            }
        }

        /// <summary> Retrieves the Activity notifications from the given Publisher for the notification
        /// bucket at the given date. This method can be invoked against all publishers that have a public timeline.
        /// For an up-to-date list of publishers that have a public timeline, see the
        /// &lt;a href="https://prod.gnipcentral.com"&t; Gnip Developer%lt;/a&gt; website.
        /// 
        /// Remember, notifications are just that -- notifications that an activity occurred on a Publisher.
        /// A notification does not contain an activity's complete data.  To obtain full activity data,
        /// use a Filter that has Filter.IsFullData = true}.
        /// </summary>
        /// <param name="publisher">the publisher whose notifications to get</param>
        /// <param name="dateTime">timestamp -  DateTime.MinValue insures that notifications are
        /// read from the latest bucket. Refer to the TimeCorrection property for more 
        /// information about the dateTime parameter.</param>
        /// <returns> the Activities model, which contains a set of Activity objects, or an empty
        /// Activities object if no notifications were found in the current notification bucket.</returns>
        /// <throws> GnipException if the publisher doesn't exist, if there were 
        /// problems authenticating with the Gnip server, or if another error occurred.</throws>
        public Activities GetNotifications(Publisher publisher, DateTime dateTime)
        {
            try
            {
                Stream inputStream = connection.DoGet(this.GetActivityUrl(publisher, true, dateTime));
                return XmlHelper.Instance.FromXmlStream<Activities>(inputStream);
            }
            catch (WebException e)
            {
                throw new GnipException("Exception occurred getting activities", e);
            }
        }
        
        /// <summary> 
        /// Convert a Gnip model object such as a Publisher or a Filter to XML and then
        /// into a byte array.  If the Config is configured to use compression, the byte array will
        /// be gzipp'ed.
        /// </summary>
        /// <param name="resource">the resource to convert</param>
        /// <returns>a byte array that represents the serialized XML document and may be gzipp'ed</returns>
        /// <throws>IOException when an exception occurs writing data to bytes </throws>
        private byte[] ConvertToBytes<T>(T resource)
        {
            Log.Debug("Starting data marshalling at " + DateTime.Now);
            MemoryStream byteArrayOutputStream = new MemoryStream();
            Stream stream;

            if (config.UseGzip)
            {
                stream = new GZipStream(byteArrayOutputStream, CompressionMode.Compress);
            }
            else
            {
                stream = byteArrayOutputStream;
            }

            XmlHelper.Instance.ToXmlStream<T>(resource, stream);
            stream.Flush();

            byte[] bytes = byteArrayOutputStream.ToArray();

            if (Log.IsDebugEnabled)
            {
                Log.Debug("Finished data marshalling at " + DateTime.Now);
                Log.Debug("data " + StringUtils.ToString(bytes));
            }

            return bytes;
        }

        /// <summary>
        /// Gets the publishers Url
        /// </summary>
        /// <param name="type">The publisher type.</param>
        /// <returns>the publishers Url</returns>
        private string GetPublishersUrl(PublisherType type)
        {
            string space = string.Empty;
            switch(type)
            {
                case PublisherType.Gnip:
                    space = "gnip";
                    break;
                case PublisherType.My:
                    space = "my";
                    break;
            }

            return config.GnipServer + space + "/publishers";
        }

        /// <summary>
        /// Gets the publishers Url for the specifies publisher name.
        /// </summary>
        /// <param name="type">The publisher type.</param>
        /// <param name="publisherName">the publishers name.</param>
        /// <returns>the publishers Url</returns>
        private string GetPublishersUrl(PublisherType publisherType, string publisherName)
        {
            return this.GetPublishersUrl(publisherType) + "/" + publisherName + ".xml";
        }

        /// <summary>
        /// Gets the publisher Url for the specifies publisher name.
        /// </summary>
        /// <param name="type">The publisher type.</param>
        /// <param name="publisherName">the publisher name.</param>
        /// <returns>the publisher Url</returns>
        private string GetPublisherUrl(PublisherType publisherType, string publisherName)
        {
            return this.GetPublishersUrl(publisherType) + "/" + publisherName;
        }

        /// <summary>
        /// Gets the Create Filter Url for the specifies publisher name.
        /// </summary>
        /// <param name="type">The publisher type.</param>
        /// <param name="publisherName">the publisher name.</param>
        /// <returns>the Create Filter Url</returns>
        private string GetFilterCreateUrl(PublisherType publisherType, string publisherName)
        {
            return this.GetPublishersUrl(publisherType) + "/" + publisherName + "/filters";
        }

        /// <summary>
        /// Gets the Get Filter Url for the specifies publisher name.
        /// </summary>
        /// <param name="type">The publisher type.</param>
        /// <param name="publisherName">the publisher name.</param>
        /// <param name="filterName">the filter name.</param>
        /// <returns>the Get Filter Url</returns>
        private string GetFilterUrl(PublisherType publisherType, string publisherName, string filterName)
        {
            return this.GetPublishersUrl(publisherType) + "/" + publisherName + "/filters/" + filterName + ".xml";
        }

        /// <summary>
        /// Gets the Get Rules Url for the specifies publisher name and filter name.
        /// </summary>
        /// <param name="type">The publisher type.</param>
        /// <param name="publisherName">the publisher name.</param>
        /// <param name="filterName">the filter name.</param>
        /// <returns>the Get Rules Url</returns>
        private string GetRulesUrl(PublisherType publisherType, string publisherName, string filterName)
        {
            return this.GetPublishersUrl(publisherType) + "/" + publisherName + "/filters/" + filterName + "/rules";
        }

        /// <summary>
        /// Gets the Delete Rules Url for the specifies publisher and filter amd rule.
        /// </summary>
        /// <param name="publisher">the publisher.</param>
        /// <param name="filterName">the name of the filter.</param>
        /// <param name="rule">the rule.</param>
        /// <returns>the Delete Rules Url</returns>
        private string GetRulesDeleteUrl(Publisher publisher, string filterName, Rule rule)
        {
            string url = this.GetRulesUrl(publisher.Type, publisher.Name, filterName);
            if (config.TunnelOverPost)
            {
                url = this.TunnelDeleteOverPost(url);
            }
            return url + "?type=" + this.EncodeUrlParameter(rule.Type.ToString().ToLower()) + "&value=" + this.EncodeUrlParameter(rule.Value);
        }

        /// <summary>
        /// Gets the Publish Activities Url for the specifies publisher.
        /// </summary>
        /// <param name="publisher">the publisher.</param>
        /// <returns>the Publish Activities Url</returns>
        private string GetActivitiesPublishUrl(Publisher publisher)
        {
            return this.GetPublisherUrl(publisher.Type, publisher.Name) + "/activity";
        }

        /// <summary>
        /// Gets the Publish Activities Url for the specifies publisher.
        /// </summary>
        /// <param name="publisher">the publisher.</param>
        /// <param name="isNotification">true if a notification, otherwise an activity.</param>
        /// <param name="date">the date. if DateTime.MinValue get the current activity.</param>
        /// <returns>the Get Activity Url</returns>
        private string GetActivityUrl(Publisher publisher, bool isNotification, DateTime date)
        {
            string bucket = (date == DateTime.MinValue ? "current" : this.GetDateString(date));
            string endpoint = isNotification ? "notification" : "activity";
            return this.GetPublisherUrl(publisher.Type, publisher.Name) + "/" + endpoint + "/" + bucket + ".xml";
        }

        /// <summary>
        /// Gets the Publish Activities Url for the specifies publisher.
        /// </summary>
        /// <param name="publisher">the publisher.</param>
        /// <param name="filter">the filter. if filter.IsFullData = true, get activity, otherwise get notification.</param>
        /// <param name="date">the date. if DateTime.MinValue get the current activity.</param>
        /// <returns>the Get Activity Url</returns>
        private string GetActivityUrl(Publisher publisher, Filter filter, DateTime date)
        {
            string bucket = (date == DateTime.MinValue ? "current" : this.GetDateString(date));
            string endpoint = filter.IsFullData ? "activity" : "notification";
            return this.GetFilterCreateUrl(publisher.Type, publisher.Name) + "/" + filter.Name + "/" + endpoint + "/" + bucket + ".xml";
        }

        /// <summary>
        /// Adds the edit tunnel param to the url.
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>The modifies url for edit tunneling.</returns>
        private string TunnelEditOverPost(string url)
        {
            return url + ";edit";
        }

        /// <summary>
        /// Adds the delete tunnel param to the url.
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>The modifies url for delete tunneling.</returns>
        private string TunnelDeleteOverPost(string url)
        {
            return url + ";delete";
        }

        /// <summary>
        /// Encodes the Url for transmission.
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>The modifies url for transmission.</returns>
        private string EncodeUrlParameter(string url)
        {
            return HttpUtility.UrlEncode(url, Encoding.UTF8);
        }

        /// <summary>
        /// Gets the date string for date.
        /// </summary>
        /// <param name="date">the date</param>
        /// <returns>a string rep. of date.</returns>
        private string GetDateString(DateTime date)
        {
            DateTime flooredDate = GnipConnection.GetBucketFloor(date.Add(this.timeCorrection).ToUniversalTime());
            return flooredDate.ToString("yyyyMMddHHmm");
        }

        public static DateTime GetBucketFloor(DateTime date)
        {
            long floor = (long)Math.Floor((double)date.Ticks / (BUCKET_SIZE * 10000));
            return new DateTime(floor * BUCKET_SIZE * 10000, DateTimeKind.Utc);
        }
    }
}