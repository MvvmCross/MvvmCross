// MvxTabBarViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTabBarViewController
        : MvxEventSourceTabBarController
          , IMvxTouchView
    {
        protected MvxTabBarViewController()
        {
            this.AdaptForBinding();
        }

        protected MvxTabBarViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get
            {
                // special code needed in TabBar because View is initialized during construction
                if (BindingContext == null) return null;
                return BindingContext.DataContext;
            }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
			get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }
    }
}