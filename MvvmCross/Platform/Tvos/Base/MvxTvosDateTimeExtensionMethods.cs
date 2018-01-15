// MvxTvosDateTimeExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Foundation;

namespace MvvmCross.Platform.tvOS
{
    public static class MvxTvosDateTimeExtensionMethods
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
    }
}