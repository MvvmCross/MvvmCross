using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public class MvxCachingFragmentActivity
        : MvxFragmentActivity
    {
        private const string SavedFragmentIndexStateKey = "__mvxSavedFragmentIndex";
        private readonly Dictionary<string, FragmentInfo> _lookup = new Dictionary<string, FragmentInfo>();
        private readonly Dictionary<int, string> _currentFragments = new Dictionary<int, string>();

        public void RegisterFragment<TFragment>(string tag)
        {
            var fragInfo = new FragmentInfo(tag, typeof (TFragment));
            
            _lookup.Add(tag, fragInfo);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            if (savedInstanceState != null)
            {
                var json = savedInstanceState.GetString(SavedFragmentIndexStateKey);
                var ser = Mvx.Resolve<IMvxJsonConverter>();

                var dict = ser.DeserializeObject<Dictionary<int, string>>(json);
                foreach (var entry in dict)
                    ShowFragment(entry.Value, entry.Key);
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (_currentFragments.Any())
            {
                var ser = Mvx.Resolve<IMvxJsonConverter>();
                var json = ser.SerializeObject(_currentFragments);

                outState.PutString(SavedFragmentIndexStateKey, json);
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

                // if we haven't already created a Fragment, do it now
                if (newFrag.CachedFragment == null)
                {
                    newFrag.CachedFragment = Fragment.Instantiate(this, FragmentJavaName(newFrag.FragmentType),
                        bundle);

                    ft.Add(contentId, newFrag.CachedFragment, newFrag.Tag);
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
            public Fragment CachedFragment { get; set; }
            public int ContentId { get; set; }

            public FragmentInfo(string tag, Type fragmentType)
            {
                Tag = tag;
                FragmentType = fragmentType;
            }
        }
    }
}