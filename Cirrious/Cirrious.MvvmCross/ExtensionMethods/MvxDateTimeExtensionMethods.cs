#region Copyright
// <copyright file="MvxDateTimeExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;

namespace Cirrious.MvvmCross.ExtensionMethods
{
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