using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Gnip.Client.Resource
{
    /// <summary> 
    /// Represents a message returned to a client from the Gnip server.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "result")]
    public class Result : MessageBase
    {
        public static readonly string Success = "Success";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Result() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">the result message</param>
        public Result(string message) : base(message) {}

        /// <summary>
        /// Returns true if the Result is a success.
        /// </summary>
        public bool IsSuccess { get { return Result.Success.Equals(this.Message); } }
    }
}
