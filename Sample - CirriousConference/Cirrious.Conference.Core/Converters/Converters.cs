using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.Visibility;

namespace Cirrious.Conference.Core.Converters
{
    public class Converters
    {
        public readonly TimeAgoValueConverter TimeAgo = new TimeAgoValueConverter();
        public readonly SessionSmallDetailsValueConverter SessionSmallDetails = new SessionSmallDetailsValueConverter();
        public readonly SimpleDateValueConverter SimpleDate = new SimpleDateValueConverter();
        public readonly SponsorImageValueConverter SponsorImage = new SponsorImageValueConverter();
        public readonly MvxVisibilityValueConverter Visibility = new MvxVisibilityValueConverter();
        public readonly MvxLanguageConverter Language = new MvxLanguageConverter();
    }
}