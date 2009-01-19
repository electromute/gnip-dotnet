using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client.Resource
{
    [TestFixture]
    public class PlaceTest : BaseResourceTest
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
        public void TestPlaceConstructor_01()
        {
            DateTime now = DateTime.Now;

        //    private double[] point;
        //private double? elevation;
        //private int? floor;
        //private string featuretypetag;
        //private string featurename;
        //private string relationshiptag;

            Place place = new Place();
            place.Point = new double[] { 10.0, 20.0 };
            place.Elevation = 2342.0;
            place.Floor = 2212;
            place.FeatureTypeTag = "FeatureTypeTag";
            place.FeatureName = "FeatureName";
            place.RelationshipTag = "RelationshipTag";

            Assert.AreEqual(2, place.Point.Length);
            Assert.AreEqual(10.0, place.Point[0]);
            Assert.AreEqual(20.0, place.Point[1]);
            Assert.AreEqual(2342.0, place.Elevation);
            Assert.AreEqual(2212, place.Floor);
            Assert.AreEqual("FeatureTypeTag", place.FeatureTypeTag);
            Assert.AreEqual("FeatureName", place.FeatureName);
            Assert.AreEqual("RelationshipTag", place.RelationshipTag);

            Place place2 = new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag");

            Assert.AreEqual(2, place2.Point.Length);
            Assert.AreEqual(10.0, place2.Point[0]);
            Assert.AreEqual(20.0, place2.Point[1]);
            Assert.AreEqual(2342.0, place2.Elevation);
            Assert.AreEqual(2212, place2.Floor);
            Assert.AreEqual("FeatureTypeTag", place2.FeatureTypeTag);
            Assert.AreEqual("FeatureName", place2.FeatureName);
            Assert.AreEqual("RelationshipTag", place2.RelationshipTag);

            Assert.IsTrue(place.DeepEquals(place2));

        }

        private void TestDeepEquals(Place objectA, Place objectB, bool expect, bool expectDeep)
        {
            Assert.AreEqual(expectDeep, objectA.DeepEquals(objectB));
            Assert.AreEqual(expectDeep, objectB.DeepEquals(objectA));
            Assert.AreEqual(expect, objectA.Equals(objectB));
            Assert.AreEqual(expect, objectB.Equals(objectA));
        }

        [Test]
        public void TestPlaceConstructor_02()
        {
            this.TestDeepEquals(
                new Place(new double[] { 10.0, 20.0 }),
                new Place(new double[] { 10.0, 20.0 }),
                false, true);

            this.TestDeepEquals(
                new Place(new double[] { 20.0, 20.0 }),
                new Place(new double[] { 10.0, 20.0 }),
                false, false);

            this.TestDeepEquals(
                new Place(new double[] { 10.0, 30.0 }),
                new Place(new double[] { 10.0, 20.0 }),
                false, false);
        }

        [Test]
        public void TestPlaceConstructor_03()
        {
            this.TestDeepEquals(
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                false, true);

            this.TestDeepEquals(
                new Place(new double[] { 20.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                false, false);

            this.TestDeepEquals(
                new Place(new double[] { 10.0, 30.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                false, false);

            this.TestDeepEquals(
                new Place(new double[] { 10.0, 20.0 }, 2343.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                false, false);

            this.TestDeepEquals(
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2213, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                false, false);

            this.TestDeepEquals(
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag 1", "FeatureName", "RelationshipTag"),
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                false, false);

            this.TestDeepEquals(
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName 1", "RelationshipTag"),
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                false, false);

            this.TestDeepEquals(
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag 1"),
                new Place(new double[] { 10.0, 20.0 }, 2342.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag"),
                false, false);
        }

        [Test]
        public void TestPlaceSerialize_01()
        {
            Place place = new Place();

            string str = XmlHelper.Instance.ToXmlString<Place>(place);
            //Console.WriteLine(str);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<place xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" />",
                            str);
        }

        [Test]
        public void TestPlaceSerialize_02()
        {
            Place place = new Place(new double[] { 10.0, 20.0 }, 2343.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag");

            string str = XmlHelper.Instance.ToXmlString<Place>(place);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<place xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                            "<point>10 20</point>" +
                            "<elev>2343</elev>" +
                            "<floor>2212</floor>" +
                            "<featuretypetag>FeatureTypeTag</featuretypetag>" +
                            "<featurename>FeatureName</featurename>" +
                            "<relationshiptag>RelationshipTag</relationshiptag>" +
                            "</place>",
                            str);
        }

        [Test]
        public void TestPlaceDeserialize_01()
        {
            Place place = new Place(new double[] { 10.0, 20.0 }, 2343.0, 2212, "FeatureTypeTag", "FeatureName", "RelationshipTag");

            string str = XmlHelper.Instance.ToXmlString<Place>(place);
            Place des = XmlHelper.Instance.FromXmlString<Place>(str);
            Assert.IsTrue(place.DeepEquals(des));
        }
    }
}
