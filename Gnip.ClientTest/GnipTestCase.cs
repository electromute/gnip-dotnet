using System;
using System.Threading;
using log4net;
using Gnip.Client.Resource;
using NUnit.Framework;
using System.IO.Compression;
using System.IO;
using Gnip.Client.Utils;
using log4net.Config;
using System.Collections.Generic;

namespace Gnip.Client
{
	public abstract class GnipTestCase : BaseTestCase
	{
		protected internal static TestConfig testConfig = TestConfig.Instance;
		
		protected internal Config config;
		protected internal GnipConnection gnipConnection;
		protected internal Filter filterToCreate;
		protected internal Filter notificationFilterToCreate;
		protected internal Activities activities;
		protected internal Publisher localPublisher;
		protected internal Activity activity1;
		protected internal Activity activity2;
		protected internal Activity activity3;

        private static ILog Log = LogManager.GetLogger(typeof(GnipTestCase));

		public override void SetUp()
		{
            XmlHelper.Instance.ValidateXml = true;
            XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("gnip.log4net.xml"));

			Log.Debug("========== Test setUp() start");
			Log.Debug("Attempting to connect to Gnip at " +  testConfig.Host + " using username " + testConfig.Username);

			config = new Config(testConfig.Username, 
                testConfig.Password, 
                new System.Uri(testConfig.Host), 
                testConfig.RequestTimeout, 
                testConfig.ReadWriteTimeout);

			gnipConnection = new GnipConnection(config);
            // Auto sync to the servers time.
            gnipConnection.TimeCorrection = gnipConnection.GetServerTimeDelta();

			string localPublisherId = testConfig.Publisher;
			localPublisher = gnipConnection.GetPublisher(testConfig.PublisherType, localPublisherId);
			if (localPublisher == null)
			{
				throw new AssertionException("No Publisher of type " + testConfig.PublisherType + " found with name " + localPublisherId + ".  Be sure " + "to provide the name of a publisher you own in the test.properties file.");
			}
			
			activities = new Activities();
			activity1 = new Activity(new Actor("joe"), "update1");
            activities.Items.Add(activity1);
			activity2 = new Activity(new Actor("tom"), "update2");
            activities.Items.Add(activity2);
			activity3 = new Activity(new Actor("jane"), "update3");
            activities.Items.Add(activity3);
			
			filterToCreate = new Filter("tomFilter");
			filterToCreate.Rules.Add(new Rule(RuleType.Actor, "tom"));
			
			notificationFilterToCreate = new Filter("janeFilter");
			notificationFilterToCreate.IsFullData = false;
			notificationFilterToCreate.Rules.Add(new Rule(RuleType.Actor, "jane"));

			Log.Debug("Test setUp() end\n");
		}
		
		public override void TearDown()
		{
			Log.Debug("Test tearDown()\n");
		}
		
		protected void WaitForServerWorkToComplete()
		{
			Thread.Sleep(testConfig.IdleMilliseconds);
		}
		
		protected string EncodePayload(string str)
		{
			MemoryStream baos = new MemoryStream();
            GZipStream gos = new GZipStream(baos, CompressionMode.Compress);
			byte[] bytes = StringUtils.ToByteArray(str);
			gos.Write(bytes, 0, bytes.Length);
            gos.Close();
			return Convert.ToBase64String(baos.ToArray());
		}
	}
}