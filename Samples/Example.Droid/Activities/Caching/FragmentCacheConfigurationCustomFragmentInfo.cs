using MvvmCross.Droid.Support.V7.Fragging.Caching;

namespace Example.Droid.Activities.Caching
{
    class FragmentCacheConfigurationCustomFragmentInfo : FragmentCacheConfiguration<MainActivityFragmentCacheInfoFactory.SerializableCustomFragmentInfo>
    {
        private readonly MainActivityFragmentCacheInfoFactory _mainActivityFragmentCacheInfoFactory;
        public FragmentCacheConfigurationCustomFragmentInfo()
        {
            _mainActivityFragmentCacheInfoFactory = new MainActivityFragmentCacheInfoFactory();
        }

        public override MvxCachedFragmentInfoFactory MvxCachedFragmentInfoFactory => _mainActivityFragmentCacheInfoFactory;

        public void RegisterFragmentsToCache()
        {
            foreach (var fragmentToRegister in _mainActivityFragmentCacheInfoFactory.GetFragmentsRegistrationData())
                RegisterFragmentToCache(fragmentToRegister.Key, fragmentToRegister.Value.FragmentType, fragmentToRegister.Value.ViewModelType);
        }
    }
}