using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.FullFragging.Caching
{
    public abstract class FragmentCacheConfiguration<TSerializableMvxCachedFragmentInfo> : IFragmentCacheConfiguration
    {
        private Dictionary<string, IMvxCachedFragmentInfo> _lookup;
        private const string SavedFragmentCacheConfiguration = "__mvxSavedFragmentCacheConfiguration";

        private const string SavedFragmentCacheConfigurationEnabledFragmentPoppedCallbackState =
            "__mvxSavedFragmentCacheConfigurationEnabledFragmentPoppedCallbackState";

        protected FragmentCacheConfiguration()
        {
            _lookup = new Dictionary<string, IMvxCachedFragmentInfo>();
            EnableOnFragmentPoppedCallback = true; //Default
            MvxCachedFragmentInfoFactory = new MvxCachedFragmentInfoFactory();
        }

        /// <summary>
        /// Enable OnFragmentPopped callback. This callback might represent a performance hit.
        /// </summary>
        public bool EnableOnFragmentPoppedCallback { get; set; }

        public virtual MvxCachedFragmentInfoFactory MvxCachedFragmentInfoFactory { get; }

        /// <summary>
        ///     Register a Fragment to be included in Caching process, this should usually be done in OnCreate
        /// </summary>
        /// <typeparam name="TFragment">Fragment Type</typeparam>
        /// <typeparam name="TViewModel">ViewModel Type</typeparam>
        /// <param name="tag">The tag of the Fragment, it is used to register it with the FragmentManager</param>
        /// <returns>True if fragment is registered, false if tag is already registered</returns>
        public bool RegisterFragmentToCache<TFragment, TViewModel>(string tag)
            where TFragment : IMvxFragmentView
            where TViewModel : IMvxViewModel
        {
            if (_lookup.ContainsKey(tag))
                return false;

            var fragInfo = MvxCachedFragmentInfoFactory.CreateFragmentInfo(tag, typeof(TFragment), typeof(TViewModel));

            _lookup.Add(tag, fragInfo);
            return true;
        }

        public bool RegisterFragmentToCache(string tag, Type fragmentType, Type viewModelType)
        {
            if (_lookup.ContainsKey(tag))
                return false;

            var fragInfo = MvxCachedFragmentInfoFactory.CreateFragmentInfo(tag, fragmentType, viewModelType);
            _lookup.Add(tag, fragInfo);
            return true;
        }

        public bool HasAnyFragmentsRegisteredToCache => _lookup.Any();

        public bool TryGetValue(string registeredFragmentTag, out IMvxCachedFragmentInfo cachedFragmentInfo)
        {
            return _lookup.TryGetValue(registeredFragmentTag, out cachedFragmentInfo);
        }

        public virtual void RestoreCacheConfiguration(Bundle savedInstanceState, IMvxJsonConverter serializer)
        {
            if (savedInstanceState == null)
                return;

            EnableOnFragmentPoppedCallback =
                savedInstanceState.GetBoolean(SavedFragmentCacheConfigurationEnabledFragmentPoppedCallbackState, true);

            // restore what fragments we have registered - and informations about registered fragments.
            var jsonSerializedMvxCachedFragmentInfosToRestore = savedInstanceState.GetString(SavedFragmentCacheConfiguration, string.Empty);

            // there are no registered fragments at this moment, skip restore
            if (string.IsNullOrEmpty(jsonSerializedMvxCachedFragmentInfosToRestore))
                return;

            var serializedMvxCachedFragmentInfos = serializer.DeserializeObject<Dictionary<string, TSerializableMvxCachedFragmentInfo>>(jsonSerializedMvxCachedFragmentInfosToRestore);
            _lookup = serializedMvxCachedFragmentInfos.ToDictionary(x => x.Key,
                (keyValuePair) => MvxCachedFragmentInfoFactory.ConvertSerializableFragmentInfo(keyValuePair.Value as SerializableMvxCachedFragmentInfo));
        }

        public virtual void SaveFragmentCacheConfigurationState(Bundle outState, IMvxJsonConverter serializer)
        {
            if (outState == null)
                return;

            var mvxCachedFragmentInfosToSave = CreateMvxCachedFragmentInfosToSave();
            string serializedMvxCachedFragmentInfosToSave = serializer.SerializeObject(mvxCachedFragmentInfosToSave);

            outState.PutString(SavedFragmentCacheConfiguration, serializedMvxCachedFragmentInfosToSave);
            outState.PutBoolean(SavedFragmentCacheConfigurationEnabledFragmentPoppedCallbackState, EnableOnFragmentPoppedCallback);
        }

        private Dictionary<string, SerializableMvxCachedFragmentInfo> CreateMvxCachedFragmentInfosToSave()
        {
            return _lookup.ToDictionary(x => x.Key, (keyValuePair) => MvxCachedFragmentInfoFactory.GetSerializableFragmentInfo(keyValuePair.Value));
        }
    }
}