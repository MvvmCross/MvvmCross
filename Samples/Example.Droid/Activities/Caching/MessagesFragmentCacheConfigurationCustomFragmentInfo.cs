using MvvmCross.Droid.Support.V7.Fragging.Caching;

namespace Example.Droid.Activities.Caching
{
    class MessagesFragmentCacheConfigurationCustomFragmentInfo : FragmentCacheConfiguration<MainActivityFragmentCacheInfoFactory.SerializableCustomFragmentInfo>
    {
        private readonly MessagesActivityFragmentCacheInfoFactory _mainActivityFragmentCacheInfoFactory;
        public MessagesFragmentCacheConfigurationCustomFragmentInfo()
        {
            _mainActivityFragmentCacheInfoFactory = new MessagesActivityFragmentCacheInfoFactory();
        }

        public override MvxCachedFragmentInfoFactory MvxCachedFragmentInfoFactory => _mainActivityFragmentCacheInfoFactory;
    }
}