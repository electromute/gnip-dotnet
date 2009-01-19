using System;
using System.Xml.Serialization;

namespace Gnip.Client.Resource
{
	/// <summary> 
    /// Represents an error returned to a client from the Gnip server.  This object provides a way to get any
	/// error information that the server sent in response to a request that caused a server-side error.
	/// </summary>
    [Serializable]
    [XmlRoot(ElementName = "error")]
    public class Error : MessageBase
	{
        /// <summary>
        /// Default constructor
        /// </summary>
        public Error() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public Error(string message) : base(message) { }
	}
}