using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Gnip.Client.Util
{
    [TestFixture]
    public class ArrayUtilsTest
    {
        [Test]
        public void TestAreEqual_01()
        {
            CheckLists(null, null, true);
            CheckLists(new int[0], null, false);
            CheckLists(null, new int[0], false);
            CheckLists(new int[] { 1 }, null, false);
            CheckLists(null, new int[] { 1 }, false);
            CheckLists(
                new int[] { 1 },
                new int[] { 1 },
                true);
            CheckLists(
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 1, 2, 3, 4, 5 },
                true);
            CheckLists(
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 5, 4, 3, 2, 1 },
                false);
            CheckLists(
                new int[] { 3, 2, 5, 4, 1 },
                new int[] { 5, 1, 4, 2, 3 },
                false);
        }

        private void CheckLists(int [] listA, int [] listB, bool expected)
        {
            Assert.AreEqual(expected, ArrayUtils.AreEqual<int>(listA, listB));
        }
    }
}
