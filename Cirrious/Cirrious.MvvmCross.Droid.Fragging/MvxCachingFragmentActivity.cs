using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public class MvxCachingFragmentActivity
        : MvxFragmentActivity
    {
        private const string SavedFragmentTypesKey = "__mvxSavedFragmentTypes";
        private const string SavedCurrentFragmentsKey = "__mvxSavedCurrentFragments";
        private readonly Dictionary<string, FragmentInfo> _lookup = new Dictionary<string, FragmentInfo>();
        private Dictionary<int, string> _currentFragments = new Dictionary<int, string>();

        /// <summary>
        /// Register a Fragment to be shown, this should usually be done in OnCreate
        /// </summary>
        /// <typeparam name="TFragment">Fragment Type</typeparam>
        /// <typeparam name="TViewModel">ViewModel Type</typeparam>
        /// <param name="tag">The tag of the Fragment, it is used to register it with the FragmentManager</param>
        public void RegisterFragment<TFragment, TViewModel>(string tag)
            where TFragment : IMvxFragmentView
            where TViewModel : IMvxViewModel
        {
            var fragInfo = new FragmentInfo(tag, typeof (TFragment), typeof(TViewModel));
            
            _lookup.Add(tag, fragInfo);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            // Gabriel has blown his trumpet. Ressurect Fragments from the dead.
            if (savedInstanceState != null)
            {
                // See if Fragments were just sleeping, and repopulate the _lookup
                // with references to them.
                foreach (var fragment in SupportFragmentManager.Fragments)
                {
                    var fragmentType = fragment.GetType();
                    var lookup = _lookup.Where(x => x.Value.FragmentType == fragmentType);
                    foreach (var item in lookup.Where(item => item.Value != null))
                    {
                        // reattach fragment to lookup
                        item.Value.CachedFragment = fragment;
                    }
                }

                IMvxJsonConverter serializer;
                if (!Mvx.TryResolve(out serializer))
                {
                    Mvx.Trace("Could not resolve IMvxJsonConverter, it is going to be hard to create ViewModel cache");
                    return;
                }

                var json = savedInstanceState.GetString(SavedCurrentFragmentsKey);
                var currentFragments = serializer.DeserializeObject<Dictionary<int, string>>(json);
                _currentFragments = currentFragments;

                IMvxSavedStateConverter savedStateConverter;
                if (!Mvx.TryResolve(out savedStateConverter))
                {
                    Mvx.Trace("Could not resolve IMvxSavedStateConverter, won't be able to convert saved state");
                    return;
                }

                IMvxMultipleViewModelCache viewModelCache;
                if (!Mvx.TryResolve(out viewModelCache))
                {
                    Mvx.Trace("Could not resolve IMvxMultipleViewModelCache, won't be able to convert saved state");
                    return;
                }

                IMvxViewModelLoader viewModelLoader;
                if (!Mvx.TryResolve(out viewModelLoader))
                {
                    Mvx.Trace("Could not resolve IMvxViewModelLoader, won't be able to load ViewModel for caching");
                    return;
                }

                // Harder ressurection, just in case we were killed to death.
                json = savedInstanceState.GetString(SavedFragmentTypesKey);
                if (string.IsNullOrEmpty(json)) return;

                var savedState = serializer.DeserializeObject<Dictionary<string, Type>>(json);
                foreach (var item in savedState)
                {
                    var bundle = savedInstanceState.GetBundle(item.Key);
                    if (bundle.IsEmpty) continue;

                    var mvxBundle = savedStateConverter.Read(bundle);

                    var request = MvxViewModelRequest.GetDefaultRequest(item.Value);

                    // repopulate the ViewModel with the SavedState and cache it.
                    var vm = viewModelLoader.LoadViewModel(request, mvxBundle);
                    viewModelCache.Cache(vm);
                }
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (_lookup.Any())
            {
                IMvxSavedStateConverter savedStateConverter;

                if (!Mvx.TryResolve(out savedStateConverter))
                {
                    return;
                }

                var typesForKeys = new Dictionary<string, Type>();

                foreach (var item in _lookup)
                {
                    var fragment = item.Value.CachedFragment as IMvxFragmentView;
                    if (fragment == null) continue;

                    var mvxBundle = fragment.CreateSaveStateBundle();
                    var bundle = new Bundle();
                    savedStateConverter.Write(bundle, mvxBundle);
                    outState.PutBundle(item.Key, bundle);

                    typesForKeys.Add(item.Key, item.Value.ViewModelType);
                }

                var ser = Mvx.Resolve<IMvxJsonConverter>();
                var json = ser.SerializeObject(typesForKeys);

                outState.PutString(SavedFragmentTypesKey, json);

                json = ser.SerializeObject(_currentFragments);
                outState.PutString(SavedCurrentFragmentsKey, json);
            }
            base.OnSaveInstanceState(outState);
        }

        /// <summary>
        /// Show Fragment with a specific tag at a specific placeholder
        /// </summary>
        /// <param name="tag">The tag for the fragment to lookup</param>
        /// <param name="contentId">Where you want to show the Fragment</param>
        /// <param name="bundle">Bundle which usually contains a Serialized MvxViewModelRequest</param>
        protected void ShowFragment(string tag, int contentId, Bundle bundle = null)
        {
            if (!_lookup.ContainsKey(tag))
                throw new MvxException("Could not find tag: {0} in cache, you need to register it first.", tag);

            // Only do something if we are not currently showing the tag at the contentId
            if (!_currentFragments.ContainsKey(contentId) || 
                (_currentFragments.ContainsKey(contentId) && _currentFragments[contentId] != tag))
            {
                var newFrag = _lookup[tag];
                if (newFrag == null)
                    throw new MvxException("FragmentInfo for {0} is null! Did someone set up us the bomb?!", tag);

                var ft = SupportFragmentManager.BeginTransaction();

                // if there is a Fragment showing on the contentId we want to present at
                // remove it first.   
                var currentFragment = SupportFragmentManager.FindFragmentById(contentId);
                if (currentFragment != null)
                {
                    ft.Detach(currentFragment);
                    _currentFragments.Remove(contentId);
                }

                newFrag.ContentId = contentId;
                // if we haven't already created a Fragment, do it now
                if (newFrag.CachedFragment == null)
                {
                    newFrag.CachedFragment = Fragment.Instantiate(this, FragmentJavaName(newFrag.FragmentType),
                        bundle);

                    ft.Add(newFrag.ContentId, newFrag.CachedFragment, newFrag.Tag);
                }
                else
                    ft.Attach(newFrag.CachedFragment);

                _currentFragments[contentId] = newFrag.Tag;
                
                OnFragmentChanging(tag, ft);
                ft.Commit();
                SupportFragmentManager.ExecutePendingTransactions();
            }
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            var namespaceText = fragmentType.Namespace ?? "";
            if (namespaceText.Length > 0)
                namespaceText = namespaceText.ToLowerInvariant() + ".";
            return namespaceText + fragmentType.Name;
        }

        public virtual void OnFragmentChanging(string tag, FragmentTransaction transaction)
        {
        }

        protected class FragmentInfo
        {
            public string Tag { get; private set; }
            public Type FragmentType { get; private set; }
            public Type ViewModelType { get; private set; }
            public Fragment CachedFragment { get; set; }
            public int ContentId { get; set; }

            public FragmentInfo(string tag, Type fragmentType, Type viewModelType)
            {
                Tag = tag;
                FragmentType = fragmentType;
                ViewModelType = viewModelType;
            }
        }
    }
}