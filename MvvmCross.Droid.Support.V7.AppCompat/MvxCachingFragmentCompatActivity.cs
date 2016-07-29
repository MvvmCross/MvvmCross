// MvxCachingFragmentActivityCompat.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Shared.Caching;
using MvvmCross.Droid.Shared.Presenter;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Shared.Fragments;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    [Register("mvvmcross.droid.support.v7.appcompat.MvxCachingFragmentCompatActivity")]
    public class MvxCachingFragmentCompatActivity : MvxAppCompatActivity, IFragmentCacheableActivity, IMvxFragmentHost
    {
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";
        private const string SavedFragmentTypesKey = "__mvxSavedFragmentTypes";
        private IFragmentCacheConfiguration _fragmentCacheConfiguration;

        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            var view = MvxAppCompatActivityHelper.OnCreateView(parent, name, context, attrs);
            return view ?? base.OnCreateView(parent, name, context, attrs);
        }

        protected enum FragmentReplaceMode
        {
            NoReplace,
            ReplaceFragment,
            ReplaceFragmentAndViewModel
        }

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

                fragmentInfo.CachedFragment = fragment as IMvxFragmentView;
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

                if (!typesForKeys.ContainsKey(info.Tag))
                    typesForKeys.Add(info.Tag, info.ViewModelType);
            }

            return typesForKeys;
        }

        protected virtual void ReplaceFragment(FragmentTransaction ft, IMvxCachedFragmentInfo fragInfo)
        {
            ft.Replace(fragInfo.ContentId, fragInfo.CachedFragment as Fragment, fragInfo.Tag);
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
        /// <param name="forceReplaceFragment">If you want the fragment to be re-created</param>
        protected virtual void ShowFragment(string tag, int contentId, Bundle bundle, bool forceAddToBackStack = false, bool forceReplaceFragment = false)
        {
            IMvxCachedFragmentInfo fragInfo;
            FragmentCacheConfiguration.TryGetValue(tag, out fragInfo);

            IMvxCachedFragmentInfo currentFragInfo = null;
            var currentFragment = SupportFragmentManager.FindFragmentById(contentId);

            if (currentFragment != null)
                FragmentCacheConfiguration.TryGetValue(currentFragment.Tag, out currentFragInfo);

            if (fragInfo == null)
                throw new MvxException("Could not find tag: {0} in cache, you need to register it first.", tag);

            // We shouldn't replace the current fragment unless we really need to.
            FragmentReplaceMode fragmentReplaceMode = FragmentReplaceMode.ReplaceFragmentAndViewModel;
            if (!forceReplaceFragment)
                fragmentReplaceMode = ShouldReplaceCurrentFragment(fragInfo, currentFragInfo, bundle);

            if (fragmentReplaceMode == FragmentReplaceMode.NoReplace)
                return;

            var ft = SupportFragmentManager.BeginTransaction();
            OnBeforeFragmentChanging(fragInfo, ft);

            fragInfo.ContentId = contentId;

            //If we already have a previously created fragment, we only need to send the new parameters
            if (fragInfo.CachedFragment != null && fragmentReplaceMode == FragmentReplaceMode.ReplaceFragment)
            {
                (fragInfo.CachedFragment as Fragment).Arguments.Clear();
                (fragInfo.CachedFragment as Fragment).Arguments.PutAll(bundle);
            }
            else
            {
                //Otherwise, create one and cache it
                fragInfo.CachedFragment = Fragment.Instantiate(this, FragmentJavaName(fragInfo.FragmentType),
                    bundle) as IMvxFragmentView;
                OnFragmentCreated(fragInfo, ft);
            }

            ft.Replace(fragInfo.ContentId, fragInfo.CachedFragment as Fragment, fragInfo.Tag);

            //if replacing ViewModel then clear the cache after the fragment
            //has been added to the transaction so that the Tag property is not null
            //and the UniqueImmutableCacheTag property (if not overridden) has the correct value
            if (fragmentReplaceMode == FragmentReplaceMode.ReplaceFragmentAndViewModel)
            {
                var cache = Mvx.GetSingleton<IMvxMultipleViewModelCache>();
                cache.GetAndClear(fragInfo.ViewModelType, GetTagFromFragment(fragInfo.CachedFragment as Fragment));
            }

            if ((currentFragment != null && fragInfo.AddToBackStack) || forceAddToBackStack)
            {
                ft.AddToBackStack(fragInfo.Tag);
            }

            OnFragmentChanging(fragInfo, ft);
            ft.Commit();
            SupportFragmentManager.ExecutePendingTransactions();
            OnFragmentChanged(fragInfo);
        }

        protected virtual FragmentReplaceMode ShouldReplaceCurrentFragment(IMvxCachedFragmentInfo newFragment, IMvxCachedFragmentInfo currentFragment, Bundle replacementBundle)
        {
            var oldBundle = (newFragment.CachedFragment as Fragment)?.Arguments;
            if (oldBundle == null) return FragmentReplaceMode.ReplaceFragment;

            var serializer = Mvx.Resolve<IMvxNavigationSerializer>();

            var json = oldBundle.GetString(MvxFragmentsPresenter.ViewModelRequestBundleKey);
            var oldRequest = serializer.Serializer.DeserializeObject<MvxViewModelRequest>(json);
            if (oldRequest == null) return FragmentReplaceMode.ReplaceFragment;

            json = replacementBundle.GetString(MvxFragmentsPresenter.ViewModelRequestBundleKey);
            var replacementRequest = serializer.Serializer.DeserializeObject<MvxViewModelRequest>(json);
            if (replacementRequest == null) return FragmentReplaceMode.ReplaceFragment;

            var areParametersEqual = ((oldRequest.ParameterValues == replacementRequest.ParameterValues) ||
                (oldRequest.ParameterValues.Count == replacementRequest.ParameterValues.Count &&
                    !oldRequest.ParameterValues.Except(replacementRequest.ParameterValues).Any()));

            if (currentFragment?.Tag != newFragment.Tag)
            {
                return !areParametersEqual
                    ? FragmentReplaceMode.ReplaceFragmentAndViewModel
                        : FragmentReplaceMode.ReplaceFragment;
            }
            else
                return !areParametersEqual
                    ? FragmentReplaceMode.ReplaceFragmentAndViewModel
                        : FragmentReplaceMode.NoReplace;
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

        protected virtual List<IMvxCachedFragmentInfo> GetCurrentCacheableFragmentsInfo()
        {
            return GetCurrentCacheableFragments()
                    .Select(frag => GetFragmentInfoByTag(GetTagFromFragment(frag)))
                    .ToList();
        }

        protected virtual IEnumerable<Fragment> GetCurrentCacheableFragments()
        {
            var currentFragments = SupportFragmentManager.Fragments ?? Enumerable.Empty<Fragment>();

            return currentFragments
                .Where(fragment => fragment != null)
                // we are not interested in fragments which are not supposed to cache!
                .Where(fragment => fragment.GetType().IsFragmentCacheable());
        }

        protected virtual IMvxCachedFragmentInfo GetLastFragmentInfo()
        {
            var currentCacheableFragments = GetCurrentCacheableFragments().ToList();
            if (!currentCacheableFragments.Any())
                throw new InvalidOperationException("Cannot retrieve last fragment as FragmentManager is empty.");

            var lastFragment = currentCacheableFragments.Last();
            var tagFragment = GetTagFromFragment(lastFragment);

            return GetFragmentInfoByTag(tagFragment);
        }

        protected virtual string GetTagFromFragment(Fragment fragment)
        {
            var mvxFragmentView = fragment as IMvxFragmentView;

            // ReSharper disable once PossibleNullReferenceException
            // Fragment can never be null because registered fragment has to inherit from IMvxFragmentView
            return mvxFragmentView.UniqueImmutableCacheTag;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (bundle == null)
            {
                var fragmentRequestText = Intent.Extras?.GetString(ViewModelRequestBundleKey);
                if (fragmentRequestText == null)
                    return;

                var converter = Mvx.Resolve<IMvxNavigationSerializer>();
                var fragmentRequest = converter.Serializer.DeserializeObject<MvxViewModelRequest>(fragmentRequestText);

                var mvxAndroidViewPresenter = Mvx.Resolve<IMvxAndroidViewPresenter>();
                mvxAndroidViewPresenter.Show(fragmentRequest);
            }
        }

        /// <summary>
        /// Close Fragment with a specific tag at a specific placeholder
        /// </summary>
        /// <param name="tag">The tag for the fragment to lookup</param>
        /// <param name="contentId">Where you want to close the Fragment</param>
        protected virtual void CloseFragment(string tag, int contentId)
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

        protected virtual IMvxCachedFragmentInfo GetFragmentInfoByTag(string tag)
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

        protected virtual string GetFragmentTag(MvxViewModelRequest request, Bundle bundle, Type fragmentType)
        {
            // THAT won't work properly if you have multiple instance of same fragment type in same FragmentHost.
            // Override that in such cases
            return request.ViewModelType.FullName;
        }

        public virtual bool Show(MvxViewModelRequest request, Bundle bundle, Type fragmentType, MvxFragmentAttribute fragmentAttribute)
        {
            var fragmentTag = GetFragmentTag(request, bundle, fragmentType);
            FragmentCacheConfiguration.RegisterFragmentToCache(fragmentTag, fragmentType, request.ViewModelType, fragmentAttribute.AddToBackStack);

            ShowFragment(fragmentTag, fragmentAttribute.FragmentContentId, bundle);
            return true;
        }

        public virtual bool Close(IMvxViewModel viewModel)
        {
            //Workaround for closing fragments. This will not work when showing multiple fragments of the same viewmodel type in one activity
            var frag = GetCurrentCacheableFragmentsInfo().FirstOrDefault(x => x.ViewModelType == viewModel.GetType());
            if (frag == null)
            {
                return false;
            }

            // Close method can not be fully fixed at this moment. That requires some changes in main MvvmCross library
            CloseFragment(frag.Tag, frag.ContentId);
            return true;
        }
    }

    public abstract class MvxCachingFragmentCompatActivity<TViewModel>
        : MvxCachingFragmentCompatActivity
    , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxCachingFragmentCompatActivity(IntPtr ptr, JniHandleOwnership ownership) : base(ptr, ownership)
        {

        }

        protected MvxCachingFragmentCompatActivity()
        {

        }
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
