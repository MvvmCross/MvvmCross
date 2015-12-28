// MvxCachingFragmentActivityCompat.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Caching;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using MvvmCross.Droid.Support.V7.Fragging.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    [Register("MvvmCross.Droid.Support.V7.AppCompat.MvxCachingFragmentCompatActivity")]
    public class MvxCachingFragmentCompatActivity : MvxFragmentCompatActivity, IFragmentCacheableActivity, IMvxFragmentHost
    {
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";
        private const string SavedFragmentTypesKey = "__mvxSavedFragmentTypes";
        private IFragmentCacheConfiguration _fragmentCacheConfiguration;

        protected MvxCachingFragmentCompatActivity()
        {
        }

        protected MvxCachingFragmentCompatActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            if (savedInstanceState == null) return;

            IMvxJsonConverter serializer;
            if (!Mvx.TryResolve(out serializer))
            {
                Mvx.Trace(
                    "Could not resolve IMvxJsonConverter, it is going to be hard to create ViewModel cache");
                return;
            }

            FragmentCacheConfiguration.RestoreCacheConfiguration(savedInstanceState, serializer);
            // Gabriel has blown his trumpet. Ressurect Fragments from the dead.
            RestoreFragmentsCache();

            RestoreViewModelsFromBundle(serializer, savedInstanceState);
        }

        private static void RestoreViewModelsFromBundle(IMvxJsonConverter serializer, Bundle savedInstanceState)
        {
            IMvxSavedStateConverter savedStateConverter;
            IMvxMultipleViewModelCache viewModelCache;
            IMvxViewModelLoader viewModelLoader;

            if (!Mvx.TryResolve(out savedStateConverter))
            {
                Mvx.Trace("Could not resolve IMvxSavedStateConverter, won't be able to convert saved state");
                return;
            }

            if (!Mvx.TryResolve(out viewModelCache))
            {
                Mvx.Trace("Could not resolve IMvxMultipleViewModelCache, won't be able to convert saved state");
                return;
            }

            if (!Mvx.TryResolve(out viewModelLoader))
            {
                Mvx.Trace("Could not resolve IMvxViewModelLoader, won't be able to load ViewModel for caching");
                return;
            }

            // Harder ressurection, just in case we were killed to death.
            var json = savedInstanceState.GetString(SavedFragmentTypesKey);
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
                viewModelCache.Cache(vm, item.Key);
            }
        }

        private void RestoreFragmentsCache()
        {
            // See if Fragments were just sleeping, and repopulate the _lookup (which is accesed in GetFragmentInfoByTag)
            // with references to them.

            // we do not want to restore fragments which aren't tracked by our cache
            foreach (var fragment in GetCurrentCacheableFragments())
            {
                // if used tag is proper tag such that:
                // it is unique and immutable
                // and fragment is properly registered
                // then there must be exactly one matching value in _lookup fragment cache container
                var fragmentTag = GetTagFromFragment(fragment);
                var fragmentInfo = GetFragmentInfoByTag(fragmentTag);

                fragmentInfo.CachedFragment = fragment;
            }
        }

        private Dictionary<string, Type> CreateFragmentTypesDictionary(Bundle outState)
        {
            IMvxSavedStateConverter savedStateConverter;
            if (!Mvx.TryResolve(out savedStateConverter))
            {
                return null;
            }

            var typesForKeys = new Dictionary<string, Type>();

            var currentFragsInfo = GetCurrentCacheableFragmentsInfo();
            foreach (var info in currentFragsInfo)
            {
                var fragment = info.CachedFragment as IMvxFragmentView;
                if (fragment == null)
                    continue;

                var mvxBundle = fragment.CreateSaveStateBundle();
                var bundle = new Bundle();
                savedStateConverter.Write(bundle, mvxBundle);
                outState.PutBundle(info.Tag, bundle);

                typesForKeys.Add(info.Tag, info.ViewModelType);
            }

            return typesForKeys;
        }

        protected virtual void ReplaceFragment(FragmentTransaction ft, IMvxCachedFragmentInfo fragInfo)
        {
            ft.Replace(fragInfo.ContentId, fragInfo.CachedFragment, fragInfo.Tag);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            IMvxJsonConverter ser;
            if (FragmentCacheConfiguration.HasAnyFragmentsRegisteredToCache && Mvx.TryResolve(out ser))
            {
                FragmentCacheConfiguration.SaveFragmentCacheConfigurationState(outState, ser);

                var typesForKeys = CreateFragmentTypesDictionary(outState);
                if (typesForKeys == null)
                    return;

                var json = ser.SerializeObject(typesForKeys);
                outState.PutString(SavedFragmentTypesKey, json);
            }
        }

        /// <summary>
        ///     Show Fragment with a specific tag at a specific placeholder
        /// </summary>
        /// <param name="tag">The tag for the fragment to lookup</param>
        /// <param name="contentId">Where you want to show the Fragment</param>
        /// <param name="bundle">Bundle which usually contains a Serialized MvxViewModelRequest</param>
        /// <param name="forceAddToBackStack">If you want to force add the fragment to the backstack so on backbutton it will go back to it. Note: This will override IMvxCachedFragmentInfo.AddToBackStack configuration.</param>
        /// <param name="forceReplaceFragment">Force replace a fragment with the same tag at the same contentid</param>
        protected void ShowFragment(string tag, int contentId, Bundle bundle = null, bool forceAddToBackStack = false, bool forceReplaceFragment = false)
        {
            IMvxCachedFragmentInfo fragInfo;
            FragmentCacheConfiguration.TryGetValue(tag, out fragInfo);

            if (fragInfo == null)
                throw new MvxException("Could not find tag: {0} in cache, you need to register it first.", tag);

            // We shouldn't replace the current fragment unless we really need to.
            var shouldReplaceCurrentFragment = forceReplaceFragment || ShouldReplaceCurrentFragment(contentId, tag);
            if (!shouldReplaceCurrentFragment)
                return;

            var ft = SupportFragmentManager.BeginTransaction();
            OnBeforeFragmentChanging(fragInfo, ft);

            fragInfo.ContentId = contentId;

            //If we already have a previously created fragment, we only need to send the new parameters
            if (fragInfo.CacheFragment && fragInfo.CachedFragment != null)
            {
                fragInfo.CachedFragment.Arguments.Clear();
                fragInfo.CachedFragment.Arguments.PutAll(bundle);
            }
            else
            {
                //Otherwise, create one and cache it
                fragInfo.CachedFragment = Fragment.Instantiate(this, FragmentJavaName(fragInfo.FragmentType),
                    bundle);
                OnFragmentCreated(fragInfo, ft);
            }

            ReplaceFragment(ft, fragInfo);

            if (fragInfo.AddToBackStack || forceAddToBackStack)
            {
                ft.AddToBackStack(fragInfo.Tag);
            }

            OnFragmentChanging(fragInfo, ft);
            ft.Commit();

            //TODO: manager crashes sometimes
            SupportFragmentManager.ExecutePendingTransactions();
            OnFragmentChanged(fragInfo);
        }

        private bool ShouldReplaceCurrentFragment(int contentId, string tag)
        {
            var currentFragment = SupportFragmentManager.FindFragmentById(contentId);
            return currentFragment == null || currentFragment.Tag != tag;
        }

        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount >= 1)
            {
                SupportFragmentManager.PopBackStackImmediate();

                if (FragmentCacheConfiguration.EnableOnFragmentPoppedCallback)
                {
                    //NOTE(vvolkgang) this is returning ALL the frags. Should we return only the visible ones?
                    var currentFragsInfo = GetCurrentCacheableFragmentsInfo();
                    OnFragmentPopped(currentFragsInfo);
                }

                return;
            }

            base.OnBackPressed();
        }

        protected List<IMvxCachedFragmentInfo> GetCurrentCacheableFragmentsInfo()
        {
            return GetCurrentCacheableFragments()
                    .Select(frag => GetFragmentInfoByTag(GetTagFromFragment(frag)))
                    .ToList();
        }

        protected IEnumerable<Fragment> GetCurrentCacheableFragments()
        {
            var currentFragments = SupportFragmentManager.Fragments ?? Enumerable.Empty<Fragment>();

            return currentFragments
                .Where(fragment => fragment != null)
                // we are not interested in fragments which are not supposed to cache!
                .Where(fragment => fragment.GetType().IsFragmentCacheable());
        }

        protected IMvxCachedFragmentInfo GetLastFragmentInfo()
        {
            var currentCacheableFragments = GetCurrentCacheableFragments().ToList();
            if (!currentCacheableFragments.Any())
                throw new InvalidOperationException("Cannot retrieve last fragment as FragmentManager is empty.");

            var lastFragment = currentCacheableFragments.Last();
            var tagFragment = GetTagFromFragment(lastFragment);

            return GetFragmentInfoByTag(tagFragment);
        }

        protected string GetTagFromFragment(Fragment fragment)
        {
            var mvxFragmentView = fragment as IMvxFragmentView;

            // ReSharper disable once PossibleNullReferenceException
            // Fragment can never be null because registered fragment has to inherit from IMvxFragmentView
            return mvxFragmentView.UniqueImmutableCacheTag;
        }

        protected override void OnStart()
        {
            base.OnStart();

            var fragmentRequestText = Intent.Extras?.GetString(ViewModelRequestBundleKey);
            if (fragmentRequestText == null)
                return;

            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            var fragmentRequest = converter.Serializer.DeserializeObject<MvxViewModelRequest>(fragmentRequestText);

            var mvxAndroidViewPresenter = Mvx.Resolve<IMvxAndroidViewPresenter>();
            mvxAndroidViewPresenter.Show(fragmentRequest);
        }

        /// <summary>
        /// Close Fragment with a specific tag at a specific placeholder
        /// </summary>
        /// <param name="tag">The tag for the fragment to lookup</param>
        /// <param name="contentId">Where you want to close the Fragment</param>
        protected void CloseFragment(string tag, int contentId)
        {
            var frag = SupportFragmentManager.FindFragmentById(contentId);
            if (frag == null) return;

            SupportFragmentManager.PopBackStackImmediate(tag, 1);
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            var namespaceText = fragmentType.Namespace ?? "";
            if (namespaceText.Length > 0)
                namespaceText = namespaceText.ToLowerInvariant() + ".";
            return namespaceText + fragmentType.Name;
        }

        public virtual void OnBeforeFragmentChanging(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction)
        {
        }

        // Called before the transaction is commited
        public virtual void OnFragmentChanging(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction) { }

        public virtual void OnFragmentChanged(IMvxCachedFragmentInfo fragmentInfo)
        {
        }

        public virtual void OnFragmentPopped(IList<IMvxCachedFragmentInfo> currentFragmentsInfo)
        {
        }

        public virtual void OnFragmentCreated(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction)
        {
        }

        protected IMvxCachedFragmentInfo GetFragmentInfoByTag(string tag)
        {
            IMvxCachedFragmentInfo fragInfo;
            FragmentCacheConfiguration.TryGetValue(tag, out fragInfo);

            if (fragInfo == null)
                throw new MvxException("Could not find tag: {0} in cache, you need to register it first.", tag);
            return fragInfo;
        }

        public IFragmentCacheConfiguration FragmentCacheConfiguration => _fragmentCacheConfiguration ?? (_fragmentCacheConfiguration = BuildFragmentCacheConfiguration());

        public virtual IFragmentCacheConfiguration BuildFragmentCacheConfiguration()
        {
            return new DefaultFragmentCacheConfiguration();
        }

        public virtual bool Show(MvxViewModelRequest request, Bundle bundle, Type fragmentType, MvxFragmentAttribute fragmentAttribute)
        {
            var fragmentTag = GetFragmentTag(request, bundle, fragmentType);
            FragmentCacheConfiguration.RegisterFragmentToCache(fragmentTag, fragmentType, request.ViewModelType);

            ShowFragment(fragmentTag, fragmentAttribute.FragmentContentId, bundle);
            return true;
        }

        protected virtual string GetFragmentTag(MvxViewModelRequest request, Bundle bundle, Type fragmentType)
        {
            // THAT won't work properly if you have multiple instance of same fragment type in same FragmentHost.
            // Override that in such cases
            return request.ViewModelType.FullName;
        }

        public bool Close(IMvxViewModel viewModel)
        {
            // Close method can not be fixed at this moment
            // That requires some changes in main MvvmCross library

            return false;
        }
    }

    public abstract class MvxCachingFragmentCompatActivity<TViewModel>
        : MvxCachingFragmentCompatActivity
    , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}