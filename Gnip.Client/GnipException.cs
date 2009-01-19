using System;

namespace Gnip.Client
{
	
	/// <summary> 
    /// Exception thrown when problems result interacting with a Gnip server.  This exception can be thrown
	/// when data is marshalled or unmarshalled.  It can also be thrown when errors are returned from the
	/// Gnip server including responses with non-200 status codes including problems logging in, access
	/// to non-existent XML documents, etc.
	/// </summary>
	[Serializable]
	public class GnipException : Exception
	{
        public GnipException() : base() { }
		
		public GnipException(string message) : base(message) { }
		
		public GnipException(string message, Exception cause) : base(message, cause) { }
	}
}