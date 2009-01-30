using System;
using System.Collections.Generic;
using System.Text;

namespace Gnip.Client.Util
{
    /// <summary>
    /// For internal use only. Made public for testing only.
    /// </summary>
    public static class ArrayUtils
    {
        public static bool AreEqual<T>(T[] listA, T[] listB)
        {
            bool ret = true;

            if (listA != listB)
            {
                if (listA == null || listB == null || listA.Length != listB.Length)
                {
                    ret = false;
                }
                else
                {
                    for (int i = 0; i < listA.Length; i++)
                    {
                        if (ObjectUtils.AreEqual(listA[i], listB[i]) == false)
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }
    }
}
