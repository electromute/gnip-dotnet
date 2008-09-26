using NUnit.Framework;
﻿using System;

namespace Gnip.Tests.Integration
{
    [TestFixture]
    public class FilterIntegrationTest
    {
        IConnection gnip;
        Filter filter;

        [SetUp]
        public void Setup()
        {
            filter = TestFactory.Filter();
            filter.Name = "apitestcollection" + Guid.NewGuid();

            gnip = TestFactory.LiveConnection();
            gnip.Create("digg", filter);            
        }

        [TearDown]
        public void TearDown()
        {
            gnip.Delete("digg", filter.Name);
        }

        [Test]
        public void CanCreateFilter()
        {
            Filter retrievedFilter = gnip.GetFilter("digg", filter.Name);
            Assert.AreEqual(filter.Name, retrievedFilter.Name);
        }

        [Test]
        public void CanUpdateAFilter()
        {
            filter.Rules.RemoveAt(0);
            filter.Rules.Add(new Rule("actor", "jeremy"));

            gnip.Update("digg", filter);

            Assert.AreEqual(filter, gnip.GetFilter("digg", filter.Name));
        }
    }
}
