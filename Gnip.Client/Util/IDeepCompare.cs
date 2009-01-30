using System;
using System.Collections.Generic;
using System.Text;

namespace Gnip.Client.Util
{
    public interface IDeepCompare
    {
        bool DeepEquals(object o);
    }
}
