using System;
using System.Xml.Serialization;

namespace Gnip.Client.Resource
{
	
	/// <summary> 
    /// Enumeration of the rule types that are supported by Gnip Filters.
    /// </summary>
    public enum RuleType
    {
        /// <summary> 
        /// Actor rules.
        /// </summary>
        [XmlEnum(Name = "actor")]
        Actor,
        /// <summary> 
        /// Regarding rule.
        /// </summary>
        [XmlEnum(Name = "regarding")]
        Regarding,
        /// <summary> 
        /// Source rule.
        /// </summary>
        [XmlEnum(Name = "source")]
        Source,
        /// <summary> 
        /// Tag rules.
        /// </summary>
        [XmlEnum(Name = "tag")]
        Tag,
        /// <summary>
        /// To rule.
        /// </summary>
        [XmlEnum(Name = "to")]
        To
    }
}