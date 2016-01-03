namespace MvvmCross.Mac.ExtensionMethods
{
    using System;
    using Foundation;

    public static class MvxMacDateTimeExtensionMethods
    {
        private static readonly DateTime ReferenceNSDateTime = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToDateTimeUtc(this NSDate date)
        {
            return (ReferenceNSDateTime).AddSeconds(date.SecondsSinceReferenceDate);
        }

        public static NSDate ToNSDate(this DateTime date)
        {
            return NSDate.FromTimeIntervalSinceReferenceDate((date - (ReferenceNSDateTime)).TotalSeconds);
        }        
    }
}
