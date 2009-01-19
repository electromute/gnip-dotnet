using System;
using System.Collections.Generic;
using System.Text;

namespace Gnip.Client.Util
{
    /// <summary>
    /// For internal use only. Made public for testing only.
    /// </summary>
    public static class ListUtils
    {
        public static bool AreEqual<T>(IList<T> listA, IList<T> listB)
        {
            bool ret = true;

            if (listA != listB)
            {
                if (listA == null || listB == null || listA.Count != listB.Count)
                {
                    ret = false;
                }
                else
                {
                    for (int i = 0; i < listA.Count; i++)
                    {
                        if (listA[i].Equals(listB[i]) == false)
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }

        public static bool AreDeepEqual<T>(IList<T> listA, IList<T> listB) where T : IDeepCompare
        {
            bool ret = true;

            if (listA != listB)
            {
                if (listA == null || listB == null || listA.Count != listB.Count)
                {
                    ret = false;
                }
                else
                {
                    for (int i = 0; i < listA.Count; i++)
                    {
                        if (listA[i].DeepEquals(listB[i]) == false)
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }


        public static bool AreEqual<T>(IList<T> listA, IList<T> listB, long length)
        {
            bool ret = true;

            if (listA != listB)
            {
                if (listA == null || listB == null || listA.Count != listB.Count)
                {
                    ret = false;
                }
                else if (listA.Count < length)
                {
                    ret = false;
                }
                else
                {
                    for (int i = 0; i < length; i++)
                    {
                        if (listA[i].Equals(listB[i]) == false)
                        {
                            ret = false;
                        }
                    }
                }
            }
            else if (length > 0 && (listA == null || listA.Count < length))
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Determines if listA and listB are equal. The order of the items
        /// in the lists are ignored. This is a brute force algorithm. 
        /// Lists need not be sorted. O(n**2)
        /// </summary>
        /// <typeparam name="T">The content type of the lists</typeparam>
        /// <param name="listA">listA</param>
        /// <param name="listB">listB</param>
        /// <returns>true if the lists are equal.</returns>
        public static bool AreEqualIgnoreOrder<T>(IList<T> listA, IList<T> listB)
        {
            bool ret = true;

            if (listA != listB)
            {
                if (listA == null || listB == null || listA.Count != listB.Count)
                {
                    ret = false;
                }
                else
                {
                    bool[] matched = new bool[listA.Count];

                    foreach (T valueB in listB)
                    {
                        bool found = false;
                        for (int idx = 0; idx < listA.Count; idx++)
                        {
                            if (matched[idx])
                                continue;
                            else if (ObjectUtils.AreEqual(listA[idx], valueB))
                            {
                                found = true;
                                matched[idx] = true;
                                break;
                            }
                        }

                        if (found == false)
                        {
                            ret = false;
                            break;
                        }
                    }
                }
            }

            return ret;
        }
    }
}
