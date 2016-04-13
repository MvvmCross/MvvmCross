using Android.OS;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Platform.Platform;
using System;

namespace MvvmCross.Droid.Shared.Caching
{
    public interface IFragmentCacheConfiguration
    {
        /// <summary>
        /// Enable OnFragmentPopped callback. This callback might represent a performance hit.
        /// </summary>
        bool EnableOnFragmentPoppedCallback { get; set; }

        MvxCachedFragmentInfoFactory MvxCachedFragmentInfoFactory { get; }
        bool HasAnyFragmentsRegisteredToCache { get; }

        /// <summary>
        ///     Register a Fragment to be included in Caching process, this should usually be done in OnCreate
        /// </summary>
        /// <typeparam name="TFragment">Fragment Type</typeparam>
        /// <typeparam name="TViewModel">ViewModel Type</typeparam>
        /// <param name="tag">The tag of the Fragment, it is used to register it with the FragmentManager</param>
        /// <returns>True if fragment is registered, false if tag is already registered</returns>
        bool RegisterFragmentToCache<TFragment, TViewModel>(string tag)
            where TFragment : IMvxFragmentView
            where TViewModel : IMvxViewModel;

        bool RegisterFragmentToCache(string tag, Type fragmentType, Type viewModelType, bool addToBackStack = false);

        bool TryGetValue(string registeredFragmentTag, out IMvxCachedFragmentInfo cachedFragmentInfo);

        void RestoreCacheConfiguration(Bundle savedInstanceState, IMvxJsonConverter serializer);

        void SaveFragmentCacheConfigurationState(Bundle outState, IMvxJsonConverter serializer);
    }
}