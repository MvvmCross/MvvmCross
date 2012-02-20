using System;

namespace Cirrious.MvvmCross.Touch.ExtensionMethods
{
    public static class MvxTouchDateTimeExtensionMethods
    {
        private static readonly DateTime ReferenceNSDateTime = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToDateTimeUtc(this MonoTouch.Foundation.NSDate date)
        {
            return (ReferenceNSDateTime).AddSeconds(date.SecondsSinceReferenceDate);
        }

        public static MonoTouch.Foundation.NSDate ToNSDate(this DateTime date)
        {
            return MonoTouch.Foundation.NSDate.FromTimeIntervalSinceReferenceDate((date - (ReferenceNSDateTime)).TotalSeconds);
        }        
    }
}