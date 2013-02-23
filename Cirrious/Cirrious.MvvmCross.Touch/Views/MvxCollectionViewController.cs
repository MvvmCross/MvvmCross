// MvxCollectionViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Touch;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
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

        public virtual object DataContext { get; set; }

        public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel) DataContext; }
            set { DataContext = value; }
        }

        public bool IsVisible
        {
            get { return this.IsVisible(); }
        }

        public MvxShowViewModelRequest ShowRequest { get; set; }

        private readonly List<IMvxUpdateableBinding> _bindings = new List<IMvxUpdateableBinding>();

        public List<IMvxUpdateableBinding> Bindings
        {
            get { return _bindings; }
        }
    }
}