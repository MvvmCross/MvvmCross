using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Support.Fragging.Caching
{
    public class FragmentCacheConfiguration
    {
        private readonly Dictionary<string, IMvxCachedFragmentInfo> _lookup;

        public FragmentCacheConfiguration()
        {
             _lookup = new Dictionary<string, IMvxCachedFragmentInfo>();
            EnableOnFragmentPoppedCallback = true; //Default
        }

        /// <summary>
        /// Enable OnFragmentPopped callback. This callback might represent a performance hit.
        /// </summary>
        public bool EnableOnFragmentPoppedCallback { get; set; }

        /// <summary>
        ///     Register a Fragment to be included in Caching process, this should usually be done in OnCreate
        /// </summary>
        /// <typeparam name="TFragment">Fragment Type</typeparam>
        /// <typeparam name="TViewModel">ViewModel Type</typeparam>
        /// <param name="tag">The tag of the Fragment, it is used to register it with the FragmentManager</param>
        internal void RegisterFragmentToCache<TFragment, TViewModel>(string tag)
            where TFragment : IMvxFragmentView
            where TViewModel : IMvxViewModel
        {
            if (_lookup.ContainsKey(tag))
                return;

            var fragInfo = CreateFragmentInfo(tag, typeof(TFragment), typeof(TViewModel));

            _lookup.Add(tag, fragInfo);
        }

        internal void RegisterFragmentToCache(string tag, Type fragmentType, Type viewModelType)
        {
            if (_lookup.ContainsKey(tag))
                return;

            var fragInfo = CreateFragmentInfo(tag, fragmentType, viewModelType);
            _lookup.Add(tag, fragInfo);
        }

        protected virtual IMvxCachedFragmentInfo CreateFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool addToBackstack = false)
        {
            if (!typeof(IMvxFragmentView).IsAssignableFrom(fragmentType))
                throw new InvalidOperationException(string.Format("Registered fragment isn't an IMvxFragmentView. Received: {0}", fragmentType));

            if (!typeof(IMvxViewModel).IsAssignableFrom(viewModelType))
                throw new InvalidOperationException(string.Format("Registered view model isn't an IMvxViewModel. Received: {0}", viewModelType));

            return new MvxCachedFragmentInfo(tag, fragmentType, viewModelType, addToBackstack);
        }

        public bool HasAnyFragmentsRegisteredToCache => _lookup.Any();

        public bool TryGetValue(string registeredFragmentTag, out IMvxCachedFragmentInfo cachedFragmentInfo)
        {
            return _lookup.TryGetValue(registeredFragmentTag, out cachedFragmentInfo);
        }
    }
}