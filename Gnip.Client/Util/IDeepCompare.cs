using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnip.Client.Util
{
    public interface IDeepCompare
    {
        bool DeepEquals(object o);
    }
}
