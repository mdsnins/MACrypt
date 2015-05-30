using System;
using System.Collections.Generic;
using System.Text;

namespace MAHttp
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime Jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long currentTimeMillis()
        {
            long p = (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            long q = (long)(5000) + p / (long)1000;
            return q;
        }
    }
}
