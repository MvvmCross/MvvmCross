// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Google.Android.Material.BottomNavigation;

namespace MvvmCross.Platforms.Android.Views
{
    [Register("mvvmcross.platforms.android.views.MvxBottomNavigationView")]
    public class MvxBottomNavigationView : BottomNavigationView, IMvxBottomNavigationView, BottomNavigationView.IOnNavigationItemSelectedListener, AndroidX.ViewPager.Widget.ViewPager.IOnPageChangeListener
    {
        private AndroidX.ViewPager.Widget.ViewPager? _viewPager;
        public AndroidX.ViewPager.Widget.ViewPager? ViewPager
        {
            get => _viewPager;
            set
            {
                _viewPager?.RemoveOnPageChangeListener(this);
                _viewPager = value;
                _viewPager?.AddOnPageChangeListener(this);
            }
        }

        private readonly Dictionary<Type, IMenuItem> _lookup = new Dictionary<Type, IMenuItem>();

        private bool _didSetListener;

        private IMenuItem? _previousMenuItem;

        protected MvxBottomNavigationView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MvxBottomNavigationView(Context context) : base(context)
        {
        }

        public MvxBottomNavigationView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public MvxBottomNavigationView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public virtual bool DidRegisterViewModelType(Type viewModelType)
        {
            return _lookup.ContainsKey(viewModelType);
        }

        public virtual void RegisterViewModel(IMenuItem menuItem, Type viewModelType)
        {
            if (!_didSetListener)
            {
                SetOnNavigationItemSelectedListener(this);
                _didSetListener = true;
            }
            _lookup.Add(viewModelType, menuItem);
            if (_lookup.Count == 1)
            {
                OnPageSelected(0);
            }
        }

        public virtual bool OnNavigationItemSelected(IMenuItem item)
        {
            ViewPager?.SetCurrentItem(item.ItemId, true);
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SetOnNavigationItemSelectedListener(null);
                _viewPager?.RemoveOnPageChangeListener(this);
                _didSetListener = false;
                ViewPager = null;
            }

            base.Dispose(disposing);
        }

        public virtual void OnPageScrollStateChanged(int state)
        {
        }

        public virtual void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }

        public virtual void OnPageSelected(int position)
        {
            // update menu items
            _previousMenuItem?.SetChecked(false);
            Menu.GetItem(position)?.SetChecked(true);
            _previousMenuItem = Menu.GetItem(position);
        }
    }
}
