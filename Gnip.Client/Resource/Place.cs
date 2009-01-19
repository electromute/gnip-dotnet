using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Gnip.Client.Util;

namespace Gnip.Client.Resource
{
    /// <remarks/>
    [Serializable]
    [XmlRoot(ElementName = "place")]
    public class Place : IResource, IDeepCompare
    {
        private double[] point;
        private double? elevation;
        private int? floor;
        private string featuretypetag;
        private string featurename;
        private string relationshiptag;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Place()
        {
        }

        /// <summary>
        /// Constructor with a point.
        /// </summary>
        /// <param name="point">The place point.</param>
        public Place(double[] point)
        {
            this.point = point;
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
        public Place(double[] point, double? elevation, int? floor, string featuretypetag, string featurename, string relationshiptag)
        {
            this.point = point;
            this.elevation = elevation;
            this.floor = floor;
            this.featuretypetag = featuretypetag;
            this.featurename = featurename;
            this.relationshiptag = relationshiptag;
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
                if (this.point == null || this.point.Length == 0)
                {
                    return null;
                }

                StringBuilder builder = new StringBuilder();

                for (int idx = 0; idx < this.point.Length; idx++)
                {
                    builder.Append(this.point[idx]);
                    builder.Append(" ");
                }

                builder.Remove(builder.Length - 1, 1);
                return builder.ToString();
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.point = null;
                }
                else
                {
                    string[] values = value.Split(' ');
                    this.point = new double[values.Length];
                    for (int idx = 0; idx < values.Length; idx++)
                    {
                        this.point[idx] = double.Parse(values[idx]);
                    }
                }
            }
        }

        /// <summary>
        /// Gets/Sets the place point.
        /// </summary>
        [XmlIgnore]
        public double[] Point
        {
            get { return this.point; }
            set { this.point = value; }
        }

        /// <summary>
        /// Gets/Sets the elevation.
        /// </summary>
        [XmlElement(ElementName = "elev")]
        public double? Elevation
        {
            get { return this.elevation; }
            set { this.elevation = value; }
        }

        /// <summary>
        /// For XML serialization. Returns wheather or not elevation is null. If null
        /// this tells the serializer not create a XMLElement.
        /// </summary>
        [XmlIgnore]
        public bool ElevationSpecified
        {
            get { return this.elevation.HasValue; }
        }

        /// <summary>
        /// Gets/Sets the place floor.
        /// </summary>
        [XmlElement(ElementName = "floor")]
        public int? Floor
        {
            get { return this.floor; }
            set { this.floor = value; }
        }

        /// <summary>
        /// For XML serialization. Returns wheather or not floor is null. If null
        /// this tells the serializer not create a XMLElement.
        /// </summary>
        [XmlIgnore]
        public bool FloorSpecified
        {
            get { return this.floor.HasValue; }
        }

        /// <summary>
        /// Gets/Sets the feature type tag.
        /// </summary>
        [XmlElement(ElementName = "featuretypetag")]
        public string FeatureTypeTag
        {
            get { return this.featuretypetag; }
            set { this.featuretypetag = value; }
        }

        /// <summary>
        /// Gets/Sets the feature name.
        /// </summary>
        [XmlElement(ElementName = "featurename")]
        public string FeatureName
        {
            get { return this.featurename; }
            set { this.featurename = value; }
        }

        /// <summary>
        /// Gets/Sets the relationship.
        /// </summary>
        [XmlElement(ElementName = "relationshiptag")]
        public string RelationshipTag
        {
            get { return this.relationshiptag; }
            set { this.relationshiptag = value; }
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
                (ArrayUtils.AreEqual<double>(this.point, that.point) &&
                this.elevation == that.elevation &&
                this.floor == that.floor &&
                string.Equals(this.featuretypetag, that.featuretypetag) &&
                string.Equals(this.featurename, that.featurename) &&
                string.Equals(this.relationshiptag, that.relationshiptag));
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
                (this.point == that.point &&
                this.elevation == that.elevation &&
                this.floor == that.floor &&
                string.Equals(this.featuretypetag, that.featuretypetag) &&
                string.Equals(this.featurename, that.featurename) &&
                string.Equals(this.relationshiptag, that.relationshiptag));
        }

        /// <summary>
        /// The GetHashCode method is suitable for use in hashing algorithms 
        /// and data structures such as a hash table. 
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int result;
            result = (this.point != null ? this.point.GetHashCode() : 0);
            result = 31 * result + (this.elevation != null ? this.elevation.GetHashCode() : 0);
            result = 31 * result + (this.floor != null ? this.floor.GetHashCode() : 0);
            result = 31 * result + (this.featuretypetag != null ? this.featuretypetag.GetHashCode() : 0);
            result = 31 * result + (this.featurename != null ? this.featurename.GetHashCode() : 0);
            result = 31 * result + (this.relationshiptag != null ? this.relationshiptag.GetHashCode() : 0);
            return result;
        }
    }
}
