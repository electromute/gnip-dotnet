using System;
using System.Collections.Generic;
using System.Text;

namespace Gnip.Client.Util
{
    public static class ObjectUtils
    {
        public static bool AreEqual(object a, object b)
        {
            if (a == b) return true;
            if (a != null) return a.Equals(b);
            return false;
        }

        public static bool AreDeepEqual(IDeepCompare a, IDeepCompare b)
        {
            if (a == b) return true;
            if (a != null) return a.DeepEquals(b);
            return false;
        }
    }
}
