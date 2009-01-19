using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnip.Client.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Converts a string to a byte array suitable for use with gnip.
        /// </summary>
        /// <param name="str">the string to convert.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// Converts a byte array to a string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
