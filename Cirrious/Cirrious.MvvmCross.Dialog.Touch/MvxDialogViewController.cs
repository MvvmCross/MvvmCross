// MvxDialogViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Touch;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Touch.Dialog.Elements;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;

namespace Cirrious.MvvmCross.Dialog.Touch
{
    public class MvxDialogViewController
        : EventSourceDialogViewController
          , IMvxBindingTouchView
    {
        protected MvxDialogViewController(UITableViewStyle style = UITableViewStyle.Grouped,
                                          RootElement root = null,
                                          bool pushing = false)
            : base(style, root, pushing)
        {
            this.AdaptForBinding();
        }

		public object DataContext { 
			get{ return BindingContext.DataContext; }
			set { BindingContext.DataContext = value; }
		}

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

		public IMvxBaseBindingContext<UIView> BindingContext { get; set; }

        #region Extra Binding helpers just for Elements

        protected T Bind<T>(T element, string bindingDescription)
        {
            return element.Bind(this, bindingDescription);
        }

        protected T Bind<T>(T element, IEnumerable<MvxBindingDescription> bindingDescription)
        {
            return element.Bind(this, bindingDescription);
        }

        #endregion
    }
}