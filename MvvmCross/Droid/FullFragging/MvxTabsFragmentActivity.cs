// MvxTabsFragmentActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Droid.FullFragging
{
    [Register("mvvmcross.droid.fullfragging.MvxTabsFragmentActivity")]
    public abstract class MvxTabsFragmentActivity
        : Views.MvxActivity
        , TabHost.IOnTabChangeListener
    {
        private const string SavedTabIndexStateKey = "__savedTabIndex";

        private readonly Dictionary<string, TabInfo> _lookup = new Dictionary<string, TabInfo>();

        private readonly int _layoutId;
        private TabHost _tabHost;
        private TabInfo _currentTab;
        private readonly int _tabContentId;

        protected MvxTabsFragmentActivity(int layoutId, int tabContentId)
        {
            _layoutId = layoutId;
            _tabContentId = tabContentId;
        }

        protected class TabInfo
        {
            public string Tag { get; private set; }
            public Type FragmentType { get; private set; }
            public Bundle Bundle { get; private set; }
            public IMvxViewModel ViewModel { get; private set; }

            public Fragment CachedFragment { get; set; }

            public TabInfo(string tag, Type fragmentType, Bundle bundle, IMvxViewModel viewModel)
            {
                Tag = tag;
                FragmentType = fragmentType;
                Bundle = bundle;
                ViewModel = viewModel;
            }
        }

        private class TabFactory
            : Java.Lang.Object
              , TabHost.ITabContentFactory
        {
            private readonly Context _context;

            public TabFactory(Context context)
            {
                _context = context;
            }

            public View CreateTabContent(String tag)
            {
                var v = new View(_context);
                v.SetMinimumWidth(0);
                v.SetMinimumHeight(0);
                return v;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(_layoutId);
            InitializeTabHost(savedInstanceState);

            if (savedInstanceState != null)
            {
                _tabHost.SetCurrentTabByTag(savedInstanceState.GetString(SavedTabIndexStateKey));
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutString(SavedTabIndexStateKey, _tabHost.CurrentTabTag);
            base.OnSaveInstanceState(outState);
        }

        private void InitializeTabHost(Bundle args)
        {
            _tabHost = (TabHost)FindViewById(Android.Resource.Id.TabHost);
            _tabHost.Setup();

            AddTabs(args);

            if (_lookup.Any())
                OnTabChanged(_lookup.First().Key);

            _tabHost.SetOnTabChangedListener(this);
        }

        protected abstract void AddTabs(Bundle args);

        protected void AddTab<TFragment>(string tagAndSpecName, string tabName, Bundle args,
                                         IMvxViewModel viewModel)
        {
            var tabSpec = this._tabHost.NewTabSpec(tagAndSpecName).SetIndicator(tabName);
            AddTab<TFragment>(args, viewModel, tabSpec);
        }

        protected void AddTab<TFragment>(Bundle args, IMvxViewModel viewModel, TabHost.TabSpec tabSpec)
        {
            var tabInfo = new TabInfo(tabSpec.Tag, typeof(TFragment), args, viewModel);
            AddTab(this, _tabHost, tabSpec, tabInfo);
            _lookup.Add(tabInfo.Tag, tabInfo);
        }

        private static void AddTab(MvxTabsFragmentActivity activity,
                                   TabHost tabHost,
                                   TabHost.TabSpec tabSpec,
                                   TabInfo tabInfo)
        {
            // Attach a Tab view factory to the spec
            tabSpec.SetContent(new TabFactory(activity));
            String tag = tabSpec.Tag;

            // Check to see if we already have a CachedFragment for this tab, probably
            // from a previously saved state.  If so, deactivate it, because our
            // initial state is that a tab isn't shown.
            tabInfo.CachedFragment = activity.FragmentManager.FindFragmentByTag(tag);
            if (tabInfo.CachedFragment != null && !tabInfo.CachedFragment.IsDetached)
            {
                var ft = activity.FragmentManager.BeginTransaction();
                ft.Detach(tabInfo.CachedFragment);
                ft.Commit();
                activity.FragmentManager.ExecutePendingTransactions();
            }

            tabHost.AddTab(tabSpec);
        }

        public virtual void OnTabChanged(string tag)
        {
            var newTab = this._lookup[tag];
            if (_currentTab != newTab)
            {
                var ft = this.FragmentManager.BeginTransaction();
                OnTabFragmentChanging(tag, ft);
                if (_currentTab?.CachedFragment != null)
                {
                    ft.Detach(_currentTab.CachedFragment);
                }
                if (newTab != null)
                {
                    if (newTab.CachedFragment == null)
                    {
                        newTab.CachedFragment = Fragment.Instantiate(this,
                                                                     FragmentJavaName(newTab.FragmentType),
                                                                     newTab.Bundle);
                        FixupDataContext(newTab);
                        ft.Add(_tabContentId, newTab.CachedFragment, newTab.Tag);
                    }
                    else
                    {
                        FixupDataContext(newTab);
                        ft.Attach(newTab.CachedFragment);
                    }
                }

                _currentTab = newTab;
                ft.Commit();
                this.FragmentManager.ExecutePendingTransactions();
            }
        }

        protected virtual void FixupDataContext(TabInfo newTab)
        {
            var consumer = newTab.CachedFragment as IMvxDataConsumer;
            if (consumer == null)
                return;

            if (consumer.DataContext != newTab.ViewModel)
                consumer.DataContext = newTab.ViewModel;
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            return Java.Lang.Class.FromType(fragmentType).Name;
        }

        public virtual void OnTabFragmentChanging(string tag, FragmentTransaction transaction)
        {
        }
    }
}