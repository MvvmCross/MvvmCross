// MvxCachingFragmentActivity.cs
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
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Cirrious.MvvmCross.Droid.Support.Fragging
{
    public class MvxCachingFragmentActivity
        : MvxFragmentActivity
    {
        private const string SavedFragmentTypesKey = "__mvxSavedFragmentTypes";
        private readonly Dictionary<string, IMvxCachedFragmentInfo> _lookup = new Dictionary<string, IMvxCachedFragmentInfo>();

        /// <summary>
        /// Enable OnFragmentPopped callback. This callback might represent a performance hit.
        /// </summary>
        protected bool EnableOnFragmentPoppedCallback { get; set; }

        /// <summary>
        ///     Register a Fragment to be shown, this should usually be done in OnCreate
        /// </summary>
        /// <typeparam name="TFragment">Fragment Type</typeparam>
        /// <typeparam name="TViewModel">ViewModel Type</typeparam>
        /// <param name="tag">The tag of the Fragment, it is used to register it with the FragmentManager</param>
        public void RegisterFragment<TFragment, TViewModel>(string tag)
            where TFragment : IMvxFragmentView
            where TViewModel : IMvxViewModel
        {
            var fragInfo = CreateFragmentInfo<TFragment, TViewModel>(tag);

            _lookup.Add(tag, fragInfo);
        }

        public void RegisterFragment(string tag, Type fragmentType, Type viewModelType)
        {
            Contract.Requires<MvxException>(fragmentType == typeof(IMvxFragmentView), string.Format("Registered fragment isn't an IMvxFragmentView. Received: {0}", fragmentType));
            Contract.Requires<MvxException>(viewModelType == typeof(IMvxViewModel), string.Format("Registered view model isn't an IMvxViewModel. Received: {0}", viewModelType));

            var fragInfo = CreateFragmentInfo(tag, fragmentType, viewModelType);
            _lookup.Add(tag, fragInfo);
        }


        protected virtual IMvxCachedFragmentInfo CreateFragmentInfo<TFragment, TViewModel>(string tag, bool addToBackstack = false)
            where TFragment : IMvxFragmentView
            where TViewModel : IMvxViewModel
        {
            return new MvxCachedFragmentInfo(tag, typeof(TFragment), typeof(TViewModel), addToBackstack);
        }

        protected virtual IMvxCachedFragmentInfo CreateFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool addToBackstack = false)
        {
            Contract.Requires<MvxException>(fragmentType == typeof(IMvxFragmentView), string.Format("Registered fragment isn't an IMvxFragmentView. Received: {0}", fragmentType));
            Contract.Requires<MvxException>(viewModelType == typeof(IMvxViewModel), string.Format("Registered view model isn't an IMvxViewModel. Received: {0}", viewModelType));

            return new MvxCachedFragmentInfo(tag, fragmentType, viewModelType, addToBackstack);
        }

        protected MvxCachingFragmentActivity()
        {
            EnableOnFragmentPoppedCallback = true; //Default
        }

        protected MvxCachingFragmentActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            if (savedInstanceState == null) return;

            // Gabriel has blown his trumpet. Ressurect Fragments from the dead.
            RestoreFragmentsCache();

            IMvxJsonConverter serializer;
            if (!Mvx.TryResolve(out serializer))
            {
                Mvx.Trace(
                    "Could not resolve IMvxJsonConverter, it is going to be hard to create ViewModel cache");
                return;
            }

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
                viewModelCache.Cache(vm);
            }
        }

        private void RestoreFragmentsCache()
        {
            // See if Fragments were just sleeping, and repopulate the _lookup
            // with references to them.
            foreach (var fragment in SupportFragmentManager.Fragments)
            {
                if (fragment != null)
                {
                    var fragmentType = fragment.GetType();
                    var lookup = _lookup.Where(x => x.Value.FragmentType == fragmentType);
                    foreach (var item in lookup.Where(item => item.Value != null)) //NOTE(vvolkgang) wut? Value should never be null in here.
                    {
                        // reattach fragment to lookup
                        item.Value.CachedFragment = fragment;
                    }
                }
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

            var currentFragsInfo = GetCurrentFragmentsInfo();
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

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            IMvxJsonConverter ser;
            if (_lookup.Any() && Mvx.TryResolve(out ser))
            {
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
            _lookup.TryGetValue(tag, out fragInfo);

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
            if (fragInfo.CachedFragment != null)
            {
                fragInfo.CachedFragment.Arguments = bundle;
            }
            else
            {
                //Otherwise, create one and cache it
                fragInfo.CachedFragment = Fragment.Instantiate(this, FragmentJavaName(fragInfo.FragmentType),
                    bundle);
                OnFragmentCreated(fragInfo, ft);
            }

            ft.Replace(fragInfo.ContentId, fragInfo.CachedFragment, fragInfo.Tag);


            if (fragInfo.AddToBackStack || forceAddToBackStack)
            {
                ft.AddToBackStack(fragInfo.Tag);
            }

            OnFragmentChanging(fragInfo, ft);
            ft.Commit();
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

                if (EnableOnFragmentPoppedCallback)
                {
                    //NOTE(vvolkgang) this is returning ALL the frags. Should we return only the visible ones?
                    var currentFragsInfo = GetCurrentFragmentsInfo();
                    OnFragmentPopped(currentFragsInfo);
                }

                return;
            }

            base.OnBackPressed();
        }

        protected List<IMvxCachedFragmentInfo> GetCurrentFragmentsInfo()
        {
            return SupportFragmentManager.Fragments
                        .Where(frag => frag != null)
                        .Select(frag => GetFragmentInfoByTag(frag.Tag))
                        .ToList();
        }

        protected IMvxCachedFragmentInfo GetLastFragmentInfo()
        {
            var lastFragment = SupportFragmentManager.Fragments.Last();

            return GetFragmentInfoByTag(lastFragment.Tag);
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

        public virtual void OnBeforeFragmentChanging(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction) { }

        // Called before the transaction is commited
        public virtual void OnFragmentChanging(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction) { }
        public virtual void OnFragmentChanged(IMvxCachedFragmentInfo fragmentInfo) { }
        public virtual void OnFragmentPopped(IList<IMvxCachedFragmentInfo> currentFragmentsInfo) { }
        public virtual void OnFragmentCreated(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction) { }

        protected IMvxCachedFragmentInfo GetFragmentInfoByTag(string tag)
        {
            IMvxCachedFragmentInfo fragInfo;
            _lookup.TryGetValue(tag, out fragInfo);

            if (fragInfo == null)
                throw new MvxException("Could not find tag: {0} in cache, you need to register it first.", tag);
            return fragInfo;
        }
    }

    public abstract class MvxCachingFragmentActivity<TViewModel>
        : MvxCachingFragmentActivity
    , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}