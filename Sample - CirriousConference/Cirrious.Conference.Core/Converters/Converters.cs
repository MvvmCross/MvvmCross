using Cirrious.MvvmCross.Localization.Converters;
using Cirrious.MvvmCross.Plugins.Visibility;

namespace Cirrious.Conference.Core.Converters
{
    public class Converters
    {
        public readonly TimeAgoValueConverter TimeAgo = new TimeAgoValueConverter();
        public readonly SessionSmallDetailsValueConverter SessionSmallDetails = new SessionSmallDetailsValueConverter();
        public readonly SimpleDateValueConverter SimpleDate = new SimpleDateValueConverter();
        public readonly SponsorImageValueConverter SponsorImage = new SponsorImageValueConverter();
        public readonly MvxVisibilityConverter Visibility = new MvxVisibilityConverter();
        public readonly MvxLanguageBinderConverter Language = new MvxLanguageBinderConverter();
    }
}