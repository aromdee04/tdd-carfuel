using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNX.Shared
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;

        public static void SetDateTime(DateTime dt)
        {
            Now = () => dt;
        }

        public static void Reset()
        {
            Now = () => DateTime.Now;
        }
    }
}
