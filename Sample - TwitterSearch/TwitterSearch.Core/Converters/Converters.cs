using Cirrious.MvvmCross.Plugins.Visibility;

namespace TwitterSearch.Core.Converters
{
    public class Converters
    {
        public readonly TimeAgoConverter TimeAgo = new TimeAgoConverter();
        public readonly MvxVisibilityConverter Visibility = new MvxVisibilityConverter();
        public readonly MvxInvertedVisibilityConverter InvertedVisibility = new MvxInvertedVisibilityConverter();		
    }
}