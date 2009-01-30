using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Gnip.Client.Util
{
    internal static class StreamUtils
    {
        /// <summary>
        /// Coppies from the inputStream to the outputStream until no more
        /// bytes are read from the inputStream.
        /// </summary>
        /// <param name="inputStream">the inputStream</param>
        /// <param name="outputStream">the outputStream</param>
        public static void Pump(Stream inputStream, Stream outputStream)
        {
            byte [] buffer = new byte[1024];

            int bytesRead = 0;
            do
            {
                bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                outputStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);
        }
    }
}
