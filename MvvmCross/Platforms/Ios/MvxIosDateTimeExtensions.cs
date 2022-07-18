// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Foundation;

namespace MvvmCross.Platforms.Ios
{
    public static class MvxIosDateTimeExtensions
    {
        private static readonly DateTime ReferenceNSDateTime = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToDateTimeUtc(this NSDate date)
        {
            return ReferenceNSDateTime.AddSeconds(date.SecondsSinceReferenceDate);
        }

        public static NSDate ToNSDate(this DateTime date)
        {
            return NSDate.FromTimeIntervalSinceReferenceDate((date - ReferenceNSDateTime).TotalSeconds);
        }

        public static DateTime WithKind(this DateTime date, DateTimeKind kind)
        {
            return new DateTime(date.Ticks, kind);
        }
    }
}
