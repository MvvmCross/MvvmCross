// MvxCollectionViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxCollectionViewController
        : MvxEventSourceCollectionViewController
          , IMvxBindingTouchView
    {
        protected MvxCollectionViewController(UICollectionViewLayout layout)
            : base(layout)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel) DataContext; }
            set { DataContext = value; }
        }

        public MvxShowViewModelRequest ShowRequest { get; set; }

        public IMvxBindingContext BindingContext { get; set; }
    }
}