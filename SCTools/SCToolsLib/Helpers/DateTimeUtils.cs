using System;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    public static class DateTimeUtils
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTimeSeconds(long seconds) => _epoch.AddSeconds(seconds);

        public static long ToUnixTimeSeconds(DateTime date)
        {
            var epoch = date - _epoch;
            return (long)epoch.TotalSeconds;
        }
    }
}
