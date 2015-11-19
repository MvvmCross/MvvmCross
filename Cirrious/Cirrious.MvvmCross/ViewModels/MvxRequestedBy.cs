// MvxRequestedBy.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxRequestedBy
    {
        public static MvxRequestedBy Unknown = new MvxRequestedBy(MvxRequestedByType.Unknown);
        public static MvxRequestedBy Bookmark = new MvxRequestedBy(MvxRequestedByType.Bookmark);
        public static MvxRequestedBy UserAction = new MvxRequestedBy(MvxRequestedByType.UserAction);

        public MvxRequestedBy()
            : this(MvxRequestedByType.Unknown)
        {
        }

        public MvxRequestedBy(MvxRequestedByType requestedByType)
            : this(requestedByType, null)
        {
        }

        public MvxRequestedBy(MvxRequestedByType requestedByType, string additionalInfo)
        {
            Type = requestedByType;
            AdditionalInfo = additionalInfo;
        }

        public MvxRequestedByType Type { get; set; }
        public string AdditionalInfo { get; set; }
    }
}