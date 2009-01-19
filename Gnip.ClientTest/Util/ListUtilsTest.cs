using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Gnip.Client.Util
{
    [TestFixture]
    public class ListUtilsTest
    {
        [Test]
        public void TestAreEqual_01()
        {
            CheckLists(null, null, true);
            CheckLists(new List<int>(), null, false);
            CheckLists(null, new List<int>(), false);
            CheckLists(new List<int>(new int[] { 1 }), null, false);
            CheckLists(null, new List<int>(new int[] { 1 }), false);
            CheckLists(
                new List<int>(new int[] { 1 }),
                new List<int>(new int[] { 1 }),
                true);
            CheckLists(
                new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                true);
            CheckLists(
                new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                new List<int>(new int[] { 5, 4, 3, 2, 1 }),
                false);
            CheckLists(
                new List<int>(new int[] { 3, 2, 5, 4, 1 }),
                new List<int>(new int[] { 5, 1, 4, 2, 3 }),
                false);
        }

        [Test]
        public void TestAreEqual_02()
        {
            CheckLists(null, null, 0, true);
            CheckLists(new List<int>(), null, 0, false);
            CheckLists(null, new List<int>(), 0, false);
            CheckLists(new List<int>(), new List<int>(), 0, true);
            CheckLists(new List<int>(new int[] { 1 }), null, 0, false);
            CheckLists(new List<int>(new int[] { 1 }), new List<int>(new int[] { 1 }), 0, true);
            CheckLists(null, new List<int>(new int[] { 1 }), 0, false);
            CheckLists(
                new List<int>(new int[] { 1 }),
                new List<int>(new int[] { 1 }),
                0, true);
            CheckLists(
                new List<int>(new int[] { 1 }),
                new List<int>(new int[] { 1 }),
                1, true);
            CheckLists(
                new List<int>(new int[] { 1 }),
                new List<int>(new int[] { 1 }),
                2, false);
            for (int i = 0; i < 6; i++)
            {
                CheckLists(
                    new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                    new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                    i, true);
            }
            CheckLists(
                    new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                    new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                    6, false);
            for (int i = 0; i < 6; i++)
            {
                CheckLists(
                    new List<int>(new int[] { 1, 2, 3, 4, 5, 6 }),
                    new List<int>(new int[] { 1, 2, 3, 4, 5, 7 }),
                    i, true);
            }
            CheckLists(
                    new List<int>(new int[] { 1, 2, 3, 4, 5, 6 }),
                    new List<int>(new int[] { 1, 2, 3, 4, 5, 7 }),
                    6, false);
            CheckLists(
                new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                new List<int>(new int[] { 5, 4, 3, 2, 1 }),
                5, false);
            CheckLists(
                new List<int>(new int[] { 3, 2, 5, 4, 1 }),
                new List<int>(new int[] { 5, 1, 4, 2, 3 }),
                5, false);
        }

        [Test]
        public void TestAreEqualIgnoreOrder_01()
        {
            CheckUnorderedLists(null, null, true);
            CheckUnorderedLists(new List<int>(), null, false);
            CheckUnorderedLists(null, new List<int>(), false);
            CheckUnorderedLists(new List<int>(new int[] { 1 }), null, false);
            CheckUnorderedLists(null, new List<int>(new int[] { 1 }), false);
            CheckUnorderedLists(
                new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                true);
            CheckUnorderedLists(
                new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                new List<int>(new int[] { 5, 4, 3, 2, 1 }),
                true);
            CheckUnorderedLists(
                new List<int>(new int[] { 3, 2, 5, 4, 1 }),
                new List<int>(new int[] { 5, 1, 4, 2, 3 }),
                true);
            CheckUnorderedLists(
                new List<int>(new int[] { 1, 1, 3, 4, 5 }),
                new List<int>(new int[] { 5, 4, 3, 2, 1 }),
                false);
            CheckUnorderedLists(
                new List<int>(new int[] { 1, 2, 3, 4, 5 }),
                new List<int>(new int[] { 5, 4, 3, 1, 1 }),
                false);
            CheckUnorderedLists(
                new List<int>(new int[] { 1, 1, 1, 1, 1 }),
                new List<int>(new int[] { 1, 1, 1, 1, 1 }),
                true);
            CheckUnorderedLists(
                new List<int>(new int[] { 1, 1, 2, 1, 1 }),
                new List<int>(new int[] { 1, 1, 1, 1, 2 }),
                true);
        }

        private void CheckUnorderedLists(List<int> listA, List<int> listB, bool expected)
        {
            Assert.AreEqual(expected, ListUtils.AreEqualIgnoreOrder<int>(listA, listB));
        }

        private void CheckLists(List<int> listA, List<int> listB, bool expected)
        {
            Assert.AreEqual(expected, ListUtils.AreEqual<int>(listA, listB));
        }

        private void CheckLists(List<int> listA, List<int> listB, int length, bool expected)
        {
            Assert.AreEqual(expected, ListUtils.AreEqual<int>(listA, listB, length));
        }
    }
}
