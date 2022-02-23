// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.ViewPager.Widget;
using Java.Lang;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Platforms.Android.Views.ViewPager
{
    //http://speakman.net.nz/blog/2014/02/20/a-bug-in-and-a-fix-for-the-way-fragmentstatepageradapter-handles-fragment-restoration/
    //https://github.com/adamsp/FragmentStatePagerIssueExample/blob/master/app/src/main/java/com/example/fragmentstatepagerissueexample/app/FixedFragmentStatePagerAdapter.java
    //https://android.googlesource.com/platform/frameworks/support/+/320113721c2e14bbc2403809046fa2959a665c11/fragment/src/main/java/androidx/fragment/app/FragmentStatePagerAdapter.java
    [Register("mvvmcross.platforms.android.views.viewpager.MvxCachingFragmentPagerAdapter")]
    public abstract class MvxCachingFragmentPagerAdapter : PagerAdapter
    {
        private Fragment _currentPrimaryItem;
        private FragmentTransaction _curTransaction;
        private readonly FragmentManager _fragmentManager;
        private readonly List<Fragment> _fragments = new List<Fragment>();
        private List<string> _savedFragmentTags = new List<string>();
        private readonly List<Fragment.SavedState> _savedState = new List<Fragment.SavedState>();

        protected FragmentFactory FragmentFactory => _fragmentManager.FragmentFactory;

        protected MvxCachingFragmentPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
#if DEBUG
            FragmentManager.EnableDebugLogging(true);
#endif
        }

        protected MvxCachingFragmentPagerAdapter(FragmentManager fragmentManager)
        {
            _fragmentManager = fragmentManager;

#if DEBUG
            FragmentManager.EnableDebugLogging(true);
#endif
        }

        public abstract Fragment GetItem(int position, Fragment.SavedState fragmentSavedState = null);

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
        {
            var fragment = (Fragment)objectValue;

            if (_curTransaction == null)
                _curTransaction = _fragmentManager.BeginTransaction();

#if DEBUG
            MvxLogHost.GetLog<MvxCachingFragmentPagerAdapter>()?.Log(LogLevel.Trace,
                $"Removing item #{position}: f={objectValue} v={((Fragment)objectValue).View} t={fragment.Tag}");
#endif

            while (_savedState.Count <= position)
            {
                _savedState.Add(null);
                _savedFragmentTags.Add(null);
            }

            _savedState[position] = fragment.IsAdded ? _fragmentManager.SaveFragmentInstanceState(fragment) : null;
            _savedFragmentTags[position] = fragment.IsAdded ? fragment.Tag : null;
            _fragments[position] = null;

            _curTransaction.Remove(fragment);
        }

        public override void FinishUpdate(ViewGroup container)
        {
            if (_curTransaction == null)
                return;

            _curTransaction.CommitAllowingStateLoss();
            _curTransaction = null;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            // If we already have this item instantiated, there is nothing
            // to do.  This can happen when we are restoring the entire pager
            // from its saved state, where the fragment manager has already
            // taken care of restoring the fragments we previously had instantiated.
            if (_fragments.Count > position)
            {
                var existingFragment = _fragments.ElementAtOrDefault(position);
                if (existingFragment != null)
                    return existingFragment;
            }

            if (_curTransaction == null)
                _curTransaction = _fragmentManager.BeginTransaction();

            var fragmentTag = GetTag(position);

            Fragment.SavedState fss = null;
            if (_savedState.Count > position)
            {
                var savedTag = _savedFragmentTags.ElementAtOrDefault(position);
                if (string.Equals(fragmentTag, savedTag))
                    fss = _savedState.ElementAtOrDefault(position);
            }

            var fragment = GetItem(position, fss);
            if (fss != null)
                fragment.SetInitialSavedState(fss);

            //if fragment tag is null let's set it to something meaning full;
            if (string.IsNullOrEmpty(fragmentTag))
            {
                fragmentTag = fragment.GetType().FragmentJavaName();
            }

#if DEBUG
            MvxLogHost.GetLog<MvxCachingFragmentPagerAdapter>()?.Log(LogLevel.Trace,
                "Adding item #{position}: f={fragment} t={tag}", position, fragment, fragmentTag);
#endif

            while (_fragments.Count <= position)
                _fragments.Add(null);

            fragment.SetMenuVisibility(false);
            fragment.UserVisibleHint = false;
            _fragments[position] = fragment;
            _curTransaction.Add(container.Id, fragment, fragmentTag);

            return fragment;
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
        {
            return ((Fragment)objectValue).View == view;
        }

        public override void RestoreState(IParcelable state, ClassLoader loader)
        {
            if (state == null)
                return;

            var bundle = (Bundle)state;
            bundle.SetClassLoader(loader);
            var fss = bundle.GetParcelableArray("states");
            _savedState.Clear();
            _fragments.Clear();

            var tags = bundle.GetStringArrayList("tags");
            if (tags != null)
                _savedFragmentTags = tags.ToList();
            else
                _savedFragmentTags.Clear();

            if (fss != null)
            {
                for (var i = 0; i < fss.Length; i++)
                {
                    var parcelable = fss.ElementAt(i);
                    var savedState = parcelable.JavaCast<Fragment.SavedState>();
                    _savedState.Add(savedState);
                }
            }

            var keys = bundle.KeySet();
            foreach (var key in keys)
            {
                if (!key.StartsWith("f"))
                    continue;

                var index = Integer.ParseInt(key.Substring(1));

                if (_fragmentManager.Fragments == null) return;

                var f = _fragmentManager.GetFragment(bundle, key);
                if (f != null)
                {
                    while (_fragments.Count() <= index)
                        _fragments.Add(null);

                    f.SetMenuVisibility(false);
                    _fragments[index] = f;
                }
            }
        }

        public override IParcelable SaveState()
        {
            Bundle state = null;

            if (_savedState.Any())
            {
                state = new Bundle();

                var fss = new IParcelable[_savedState.Count];
                for (var i = 0; i < _savedState.Count; i++)
                    fss[i] = _savedState.ElementAt(i);

                state.PutParcelableArray("states", fss);
                state.PutStringArrayList("tags", _savedFragmentTags);
            }

            for (var i = 0; i < _fragments.Count; i++)
            {
                var f = _fragments.ElementAtOrDefault(i);
                if (f == null)
                    continue;

                if (state == null)
                    state = new Bundle();
                var key = "f" + i;
                _fragmentManager.PutFragment(state, key, f);
            }
            return state;
        }

        public override void SetPrimaryItem(ViewGroup container, int position, Java.Lang.Object objectValue)
        {
            var fragment = (Fragment)objectValue;
            if (fragment == _currentPrimaryItem)
                return;

            if (_currentPrimaryItem != null)
            {
                _currentPrimaryItem.SetMenuVisibility(false);
                _currentPrimaryItem.UserVisibleHint = false;
            }
            if (fragment != null)
            {
                fragment.SetMenuVisibility(true);
                fragment.UserVisibleHint = true;
            }
            _currentPrimaryItem = fragment;
        }

        protected virtual string GetTag(int position)
        {
            return null;
        }

        public override void StartUpdate(ViewGroup container)
        {
            if (container.Id == View.NoId)
            {
                throw new IllegalStateException("ViewPager with adapter " + this
                    + " requires a view id");
            }
        }
    }
}
