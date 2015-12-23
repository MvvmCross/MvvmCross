using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Cirrious.CrossCore;
using Java.Lang;
using Object = Java.Lang.Object;

namespace MvvmCross.Droid.Support.V4
{
    //http://speakman.net.nz/blog/2014/02/20/a-bug-in-and-a-fix-for-the-way-fragmentstatepageradapter-handles-fragment-restoration/
    //https://github.com/adamsp/FragmentStatePagerIssueExample/blob/master/app/src/main/java/com/example/fragmentstatepagerissueexample/app/FixedFragmentStatePagerAdapter.java
    public abstract class FixedFragmentStatePagerAdapter : PagerAdapter
    {
        private Fragment _currentPrimaryItem;
        private FragmentTransaction _curTransaction;
        private readonly FragmentManager _fragmentManager;
        private readonly List<Fragment> _fragments = new List<Fragment>();
        private List<string> _savedFragmentTags = new List<string>();
        private readonly List<Fragment.SavedState> _savedState = new List<Fragment.SavedState>();

        protected FixedFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected FixedFragmentStatePagerAdapter(FragmentManager fragmentManager)
        {
            _fragmentManager = fragmentManager;
        }

        public abstract Fragment GetItem(int position, Fragment.SavedState fragmentSavedState = null);

        public override void DestroyItem(ViewGroup container, int position, Object objectValue)
        {
            var fragment = (Fragment)objectValue;

            if (_curTransaction == null)
                _curTransaction = _fragmentManager.BeginTransaction();

#if DEBUG
            Mvx.Trace("Removing item #" + position + ": f=" + objectValue + " v=" + ((Fragment) objectValue).View +
                      " t=" + fragment.Tag);
#endif

            while (_savedState.Count <= position)
            {
                _savedState.Add(null);
                _savedFragmentTags.Add(null);
            }

            _savedState[position] = _fragmentManager.SaveFragmentInstanceState(fragment);
            _savedFragmentTags[position] = fragment.Tag;
            _fragments[position] = null;

            _curTransaction.Remove(fragment);
        }

        public override void FinishUpdate(ViewGroup container)
        {
            if (_curTransaction == null)
                return;

            _curTransaction.CommitAllowingStateLoss();
            _curTransaction = null;
            _fragmentManager.ExecutePendingTransactions();
        }

        public override Object InstantiateItem(ViewGroup container, int position)
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

#if DEBUG
            Mvx.Trace("Adding item #" + position + ": f=" + fragment + " t=" + fragmentTag);
#endif

            while (_fragments.Count <= position)
                _fragments.Add(null);

            fragment.SetMenuVisibility(false);
            fragment.UserVisibleHint = false;
            _fragments[position] = fragment;
            _curTransaction.Add(container.Id, fragment, fragmentTag);

            return fragment;
        }

        public override bool IsViewFromObject(View view, Object objectValue)
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

        public override void SetPrimaryItem(ViewGroup container, int position, Object objectValue)
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

        public override void StartUpdate(View container)
        {
        }

        protected virtual string GetTag(int position)
        {
            return null;
        }
    }
}