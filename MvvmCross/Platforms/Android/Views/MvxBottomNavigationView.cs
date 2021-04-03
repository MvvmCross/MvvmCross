// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Google.Android.Material.BottomNavigation;

namespace MvvmCross.Platforms.Android.Views
{
    [Register("mvvmcross.platforms.android.views.MvxBottomNavigationView")]
    public class MvxBottomNavigationView : BottomNavigationView, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private readonly Dictionary<IMenuItem, Type> _lookup = new Dictionary<IMenuItem, Type>();

        private bool _didSetListener;

        public MvxBottomNavigationView(Context context) : base(context)
        {
        }

        public MvxBottomNavigationView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public MvxBottomNavigationView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected MvxBottomNavigationView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public void AddItem(IMenuItem item, Type viewModel)
        {
            if (!_didSetListener)
            {
                SetOnNavigationItemSelectedListener(this);
                _didSetListener = true;
            }
            _lookup.Add(item, viewModel);

            // The first item is auto-selected
            if (_lookup.Count == 1)
            {
                OnNavigationItemSelected(item);
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var viewModelType = FindItemByMenuItem(item);

            if (viewModelType != null && HandleNavigate.CanExecute(viewModelType))
            {
                HandleNavigate.Execute(viewModelType);
                return true;
            }

            return false;
        }

        public IMenuItem FindItemByViewModel(Type viewModel)
        {
            return _lookup.FirstOrDefault(i => i.Value == viewModel).Key;
        }

        public Type FindItemByMenuItem(IMenuItem item)
        {
            return _lookup[item];
        }

        public ICommand HandleNavigate { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SetOnNavigationItemSelectedListener(null);
                _didSetListener = false;
            }

            base.Dispose(disposing);
        }
    }
}
