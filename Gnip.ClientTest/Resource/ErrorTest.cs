using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class ErrorTest : BaseResourceTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestErrorConstructor_01()
        {
            Error error = new Error();
            error.Message = "message 1";

            Error error2 = new Error("message 1");

            Assert.IsTrue(error.DeepEquals(error2));

        }

        private void TestDeepEquals(Error objectA, Error objectB, bool expect, bool expectDeep)
        {
            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestErrorConstructor_02()
        {
            this.TestDeepEquals(
                new Error("value 1"),
                new Error("value 1"),
                true, true);

            this.TestDeepEquals(
                new Error("value 2"),
                new Error("value 1"),
                false, false);
        }

        [Test]
        public void TestErrorSerialize_01()
        {
            Error error = new Error("message 1");

            string str = XmlHelper.Instance.ToXmlString<Error>(error);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<error xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">message 1</error>",
                            str);
        }

        [Test]
        public void TestErrorDeserialize_01()
        {
            Error error = new Error("message 1");

            string str = XmlHelper.Instance.ToXmlString<Error>(error);

            Error des = XmlHelper.Instance.FromXmlString<Error>(str);
            Assert.IsTrue(error.DeepEquals(des));
        }
    }
}
