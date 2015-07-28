// MvxCachingFragmentActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.FullFragging.Fragments;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using MvxActivity = Cirrious.MvvmCross.Droid.FullFragging.Views.MvxActivity;
using Android.Runtime;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace Cirrious.MvvmCross.Droid.FullFragging
{
	public class MvxCachingFragmentActivity
        : MvxActivity
	{
		private const string SavedFragmentTypesKey = "__mvxSavedFragmentTypes";
		private const string SavedCurrentFragmentsKey = "__mvxSavedCurrentFragments";
		private const string SavedBackStackFragmentsKey = "__mvxSavedBackStackFragments";
		private readonly Dictionary<string, FragmentInfo> _lookup = new Dictionary<string, FragmentInfo>();
		private Dictionary<int, string> _currentFragments = new Dictionary<int, string>();
		private IList<KeyValuePair<int, string>> _backStackFragments = new List<KeyValuePair<int, string>>();

		/// <summary>
		///     Register a Fragment to be shown, this should usually be done in OnCreate
		/// </summary>
		/// <typeparam name="TFragment">Fragment Type</typeparam>
		/// <typeparam name="TViewModel">ViewModel Type</typeparam>
		/// <param name="tag">The tag of the Fragment, it is used to register it with the FragmentManager</param>
        public void RegisterFragment<TFragment, TViewModel>(string tag)
		where TViewModel : IMvxViewModel
            where TFragment : IMvxFragmentView
		{
			var fragInfo = new FragmentInfo(tag, typeof(TFragment), typeof(TViewModel));

			_lookup.Add(tag, fragInfo);
		}

		protected MvxCachingFragmentActivity()
		{
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
            RestoreLookupFromSleep();

			IMvxJsonConverter serializer;
			if (!Mvx.TryResolve(out serializer))
			{
				Mvx.Trace(
                    "Could not resolve IMvxNavigationSerializer, it is going to be hard to create ViewModel cache");
				return;
			}

			RestoreCurrentFragmentsFromBundle(serializer, savedInstanceState);
			RestoreBackStackFragmentsFromBundle(serializer, savedInstanceState);
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

		private void RestoreCurrentFragmentsFromBundle(IMvxJsonConverter serializer, Bundle savedInstanceState)
		{
			var json = savedInstanceState.GetString(SavedCurrentFragmentsKey);
			var currentFragments = serializer.DeserializeObject<Dictionary<int, string>>(json);
			_currentFragments = currentFragments;
		}

		private void RestoreBackStackFragmentsFromBundle(IMvxJsonConverter serializer, Bundle savedInstanceState)
		{
			var jsonBackStack = savedInstanceState.GetString(SavedBackStackFragmentsKey);
			var backStackFragments = serializer.DeserializeObject<List<KeyValuePair<int, string>>>(jsonBackStack);
			_backStackFragments = backStackFragments;
		}

		private void RestoreLookupFromSleep()
		{
			// See if Fragments were just sleeping, and repopulate the _lookup
			// with references to them.
            foreach (var fragment in Fragments)
			{
				if (fragment != null) {
					var fragmentType = fragment.GetType ();
					var lookup = _lookup.Where (x => x.Value.FragmentType == fragmentType);
					foreach (var item in lookup.Where(item => item.Value != null)) {
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

			foreach (var item in _lookup)
			{
				if (_currentFragments.Any(x => x.Value == item.Key))
				{
					var fragment = item.Value.CachedFragment as IMvxFragmentView;
					if (fragment == null)
						continue;

					var mvxBundle = fragment.CreateSaveStateBundle();
					var bundle = new Bundle();
					savedStateConverter.Write(bundle, mvxBundle);
					outState.PutBundle(item.Key, bundle);

					typesForKeys.Add(item.Key, item.Value.ViewModelType);
				}
			}

			return typesForKeys;
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			if (_lookup.Any())
			{
				var typesForKeys = CreateFragmentTypesDictionary(outState);
				if (typesForKeys == null)
					return;

				IMvxJsonConverter ser;
				if (!Mvx.TryResolve(out ser))
				{
					return;
				}

				var json = ser.SerializeObject(typesForKeys);
				outState.PutString(SavedFragmentTypesKey, json);

				json = ser.SerializeObject(_currentFragments);
				outState.PutString(SavedCurrentFragmentsKey, json);
				
				json = ser.SerializeObject(_backStackFragments);
                outState.PutString(SavedBackStackFragmentsKey, json);
			}

			base.OnSaveInstanceState(outState);
		}

		/// <summary>
		///     Show Fragment with a specific tag at a specific placeholder
		/// </summary>
		/// <param name="tag">The tag for the fragment to lookup</param>
		/// <param name="contentId">Where you want to show the Fragment</param>
		/// <param name="bundle">Bundle which usually contains a Serialized MvxViewModelRequest</param>
        /// <param name="addToBackStack">If you want to add the fragment to the backstack so on backbutton it will go back to it</param>
        protected void ShowFragment(string tag, int contentId, Bundle bundle = null, bool addToBackStack = false)
		{
			FragmentInfo fragInfo;
			_lookup.TryGetValue(tag, out fragInfo);

			if (fragInfo == null)
				throw new MvxException("Could not find tag: {0} in cache, you need to register it first.", tag);

			string currentFragment;
			_currentFragments.TryGetValue(contentId, out currentFragment);

			// Only do something if we are not currently showing the tag at the contentId
            if (IsFragmentCurrentlyShowing(contentId, tag)) return;

			var ft = FragmentManager.BeginTransaction();
			OnBeforeFragmentChanging(tag, ft);

			// if there is a Fragment showing on the contentId we want to present at
			// remove it first.   
            RemoveFragmentIfShowing(ft, contentId);

			fragInfo.ContentId = contentId;
			// if we haven't already created a Fragment, do it now
            if (fragInfo.CachedFragment == null)
			{
				fragInfo.CachedFragment = Fragment.Instantiate(this, FragmentJavaName(fragInfo.FragmentType),
                    bundle);

				ft.Add(fragInfo.ContentId, fragInfo.CachedFragment, fragInfo.Tag);
			}
			else
				ft.Attach(fragInfo.CachedFragment);

			_currentFragments[contentId] = fragInfo.Tag;

            if (addToBackStack)
                ft.AddToBackStack(fragInfo.Tag);

			OnFragmentChanging(tag, ft);
			ft.Commit();
			FragmentManager.ExecutePendingTransactions();
		}

		protected virtual bool IsFragmentCurrentlyShowing(int contentId, string tag)
		{
			string currentFragment;
			_currentFragments.TryGetValue(contentId, out currentFragment);

			return currentFragment == tag;
		}

        private void RemoveFragmentIfShowing(FragmentTransaction ft, int contentId)
        {
            var frag = FragmentManager.FindFragmentById(contentId);
            if (frag == null) return;

            ft.Detach(frag);

            var currentFragment = _currentFragments.First(x => x.Key == contentId);
            _backStackFragments.Add(currentFragment);

            _currentFragments.Remove(contentId);
        }

        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount > 1)
            {
                var backStackFrag = FragmentManager.GetBackStackEntryAt(FragmentManager.BackStackEntryCount - 1);
                _currentFragments.Remove(_currentFragments.Last(x => x.Value == backStackFrag.Name).Key);

                var newFrag = FragmentManager.GetBackStackEntryAt(FragmentManager.BackStackEntryCount - 2);
                var currentFragment = _backStackFragments.Last(x => x.Value == newFrag.Name);

                _currentFragments.Add(currentFragment.Key, currentFragment.Value);
                _backStackFragments.Remove(currentFragment);

                FragmentManager.PopBackStackImmediate();
                return;
            }
            else if (FragmentManager.BackStackEntryCount == 1)
            {
                MoveTaskToBack(true);
                return;
            }

            base.OnBackPressed();
        }

        /// <summary>
        /// Close Fragment with a specific tag at a specific placeholder
        /// </summary>
        /// <param name="tag">The tag for the fragment to lookup</param>
        /// <param name="contentId">Where you want to close the Fragment</param>
        protected void CloseFragment(string tag, int contentId)
        {
            var frag = FragmentManager.FindFragmentById(contentId);
            if (frag == null) return;

            FragmentManager.PopBackStackImmediate(tag, PopBackStackFlags.Inclusive);

            _currentFragments.Remove(contentId);

            if (_backStackFragments.Count > 0 && _backStackFragments.Any(x => x.Key == contentId))
            {
                var currentFragment = _backStackFragments.Last(x => x.Key == contentId);

                _currentFragments.Add(currentFragment.Key, currentFragment.Value);
                _backStackFragments.Remove(currentFragment);
            }
        }

        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount > 1)
            {
                var backStackFrag = FragmentManager.GetBackStackEntryAt(FragmentManager.BackStackEntryCount - 1);
                _currentFragments.Remove(_currentFragments.Last(x => x.Value == backStackFrag.Name).Key);

                FragmentManager.PopBackStackImmediate();
                return;
            }
            else if (FragmentManager.BackStackEntryCount == 1)
            {
                MoveTaskToBack(true);
                return;
            }

            base.OnBackPressed();
        }

        /// <summary>
        ///     Close Fragment with a specific tag at a specific placeholder
        /// </summary>
        /// <param name="tag">The tag for the fragment to lookup</param>
        /// <param name="contentId">Where you want to close the Fragment</param>
        protected void CloseFragment(string tag, int contentId)
        {
            var frag = FragmentManager.FindFragmentById(contentId);
            if (frag == null) return;

            FragmentManager.PopBackStackImmediate(tag, PopBackStackFlags.Inclusive);
            _currentFragments.Remove(contentId);
        }

		protected virtual string FragmentJavaName(Type fragmentType)
		{
			var namespaceText = fragmentType.Namespace ?? "";
			if (namespaceText.Length > 0)
				namespaceText = namespaceText.ToLowerInvariant() + ".";
			return namespaceText + fragmentType.Name;
		}

		public virtual void OnBeforeFragmentChanging(string tag, FragmentTransaction transaction) { }

		public virtual void OnFragmentChanging(string tag, FragmentTransaction transaction) { }

		protected class FragmentInfo
		{
			public FragmentInfo(string tag, Type fragmentType, Type viewModelType)
			{
				Tag = tag;
				FragmentType = fragmentType;
				ViewModelType = viewModelType;
			}

			public string Tag { get; private set; }
			public Type FragmentType { get; private set; }
			public Type ViewModelType { get; private set; }
			public Fragment CachedFragment { get; set; }
			public int ContentId { get; set; }
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