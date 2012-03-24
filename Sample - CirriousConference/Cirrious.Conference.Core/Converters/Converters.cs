using Cirrious.MvvmCross.Converters;
using Cirrious.MvvmCross.Converters.Visibility;

namespace Cirrious.Conference.Core.Converters
{
    public class Converters
    {
        public readonly TimeAgoConverter TimeAgo = new TimeAgoConverter();
        public readonly SessionSmallDetailsValueConverter SessionSmallDetails = new SessionSmallDetailsValueConverter();
        public readonly SimpleDateValueConverter SimpleDate = new SimpleDateValueConverter();
        public readonly SponsorImageValueConverter SponsorImage = new SponsorImageValueConverter();
        public readonly MvxVisibilityConverter Visibility = new MvxVisibilityConverter();
        public readonly MvxLanguageBinderConverter Language = new MvxLanguageBinderConverter();
    }
}