// MvxDateTimeExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid.Platform
{
    using System;

    public static class MvxDateTimeExtensionMethods
    {
        private static readonly DateTime UnixZeroUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromMillisecondsUnixTimeToUtc(this long milliseconds)
        {
            var time = UnixZeroUtc;
            time = time.AddMilliseconds(milliseconds);
            return time;
        }

        public static DateTime FromUnixTimeToUtc(this long seconds)
        {
            var time = UnixZeroUtc;
            time = time.AddSeconds(seconds);
            return time;
        }

        public static DateTime FromUnixTimeToLocal(this long seconds)
        {
            return seconds.FromUnixTimeToUtc().ToLocalTime();
        }

        public static long FromUtcToUnixTime(this DateTime dateTimeUtc)
        {
            var timeSpan = (dateTimeUtc - UnixZeroUtc);
            var timestamp = (long)timeSpan.TotalSeconds;
            return timestamp;
        }

        public static long FromLocalToUnixTime(this DateTime dateTimeLocal)
        {
            return dateTimeLocal.ToUniversalTime().FromUtcToUnixTime();
        }
    }
}