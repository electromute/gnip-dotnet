using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Gnip.Client.Util;
using System.Globalization;

namespace Gnip.Client.Resource
{
    /// <remarks/>
    [Serializable]
    [XmlRoot(ElementName = "place")]
    public class Place : IResource, IDeepCompare
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Place() { }

        /// <summary>
        /// Constructor with a point.
        /// </summary>
        /// <param name="point">The place point.</param>
        public Place(double[] point)
            : this()
        {
            this.Point = point;
        }

        /// <summary>
        /// Constructor with all params.
        /// </summary>
        /// <param name="point">The place point.</param>
        /// <param name="elevation">The elevation.</param>
        /// <param name="floor">The floor.</param>
        /// <param name="featuretypetag">The Feature Type Tag</param>
        /// <param name="featurename">The Feature Name</param>
        /// <param name="relationshiptag">The Relationship Tag</param>
        public Place(double[] point, double? elevation, int? floor, string featureTypeTag, string featureName, string relationshipTag)
        {
            this.Point = point;
            this.Elevation = elevation;
            this.Floor = floor;
            this.FeatureTypeTag = featureTypeTag;
            this.FeatureName = featureName;
            this.RelationshipTag = relationshipTag;
        }

        /// <summary>
        /// Gets/Sets the place point in a string. This property is used for serialization/deserialization
        /// and should not be accessed by clients. Please use the Point property.
        /// </summary>
        [XmlElement(ElementName = "point")]
        public string PointString
        {
            get
            {
                double[] point = this.Point;

                if (point == null || point.Length == 0)
                {
                    return null;
                }

                StringBuilder builder = new StringBuilder();

                for (int idx = 0; idx < point.Length; idx++)
                {
                    builder.Append(point[idx]);
                    builder.Append(" ");
                }

                builder.Remove(builder.Length - 1, 1);
                return builder.ToString();
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.Point = null;
                }
                else
                {
                    string[] values = value.Split(' ');
                    this.Point = new double[values.Length];
                    for (int idx = 0; idx < values.Length; idx++)
                    {
                        this.Point[idx] = double.Parse(values[idx], CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        /// <summary>
        /// Gets/Sets the place point.
        /// </summary>
        [XmlIgnore]
        public double[] Point { get; set; }

        /// <summary>
        /// Gets/Sets the elevation.
        /// </summary>
        [XmlElement(ElementName = "elev")]
        public double? Elevation { get; set; }

        /// <summary>
        /// For XML serialization. Returns wheather or not elevation is null. If null
        /// this tells the serializer not create a XMLElement.
        /// </summary>
        [XmlIgnore]
        public bool ElevationSpecified
        {
            get { return this.Elevation.HasValue; }
        }

        /// <summary>
        /// Gets/Sets the place floor.
        /// </summary>
        [XmlElement(ElementName = "floor")]
        public int? Floor { get; set; }

        /// <summary>
        /// For XML serialization. Returns wheather or not floor is null. If null
        /// this tells the serializer not create a XMLElement.
        /// </summary>
        [XmlIgnore]
        public bool FloorSpecified
        {
            get { return this.Floor.HasValue; }
        }

        /// <summary>
        /// Gets/Sets the feature type tag.
        /// </summary>
        [XmlElement(ElementName = "featuretypetag")]
        public string FeatureTypeTag { get; set; }

        /// <summary>
        /// Gets/Sets the feature name.
        /// </summary>
        [XmlElement(ElementName = "featurename")]
        public string FeatureName { get; set; }

        /// <summary>
        /// Gets/Sets the relationship.
        /// </summary>
        [XmlElement(ElementName = "relationshiptag")]
        public string RelationshipTag { get; set; }

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

            return this.DeepEquals((Place)o);
        }

        /// <summary>
        /// Determins if this equals that by performing a deep equals 
        /// looking at all elements of all member listsand objects.
        /// </summary>
        /// <param name="that">The object to compare for equality.</param>
        /// <returns>True if this is equal to that, false otherwise.</returns>
        public bool DeepEquals(Place that)
        {
            if (this == that)
                return true;
            else if (that == null)
                return false;

            return
                (ArrayUtils.AreEqual<double>(this.Point, that.Point) &&
                this.Elevation == that.Elevation &&
                this.Floor == that.Floor &&
                string.Equals(this.FeatureTypeTag, that.FeatureTypeTag) &&
                string.Equals(this.FeatureName, that.FeatureName) &&
                string.Equals(this.RelationshipTag, that.RelationshipTag));
        }

        /// <summary>
        /// Determines whether the specified Object is equal to the current Object. Ths performs
        /// a deep equals.
        /// </summary>
        /// <param name="o">the specifies object</param>
        /// <returns>true if equal, false otherwise</returns>
        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (o == null || GetType() != o.GetType())
                return false;

            Place that = (Place)o;

            return
                (this.Point == that.Point &&
                this.Elevation == that.Elevation &&
                this.Floor == that.Floor &&
                string.Equals(this.FeatureTypeTag, that.FeatureTypeTag) &&
                string.Equals(this.FeatureName, that.FeatureName) &&
                string.Equals(this.RelationshipTag, that.RelationshipTag));
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result;
            result = (this.Point != null ? this.Point.GetHashCode() : 0);
            result = 31 * result + (this.Elevation != null ? this.Elevation.GetHashCode() : 0);
            result = 31 * result + (this.Floor != null ? this.Floor.GetHashCode() : 0);
            result = 31 * result + (this.FeatureTypeTag != null ? this.FeatureTypeTag.GetHashCode() : 0);
            result = 31 * result + (this.FeatureName != null ? this.FeatureName.GetHashCode() : 0);
            result = 31 * result + (this.RelationshipTag != null ? this.RelationshipTag.GetHashCode() : 0);
            return result;
        }
    }
}
