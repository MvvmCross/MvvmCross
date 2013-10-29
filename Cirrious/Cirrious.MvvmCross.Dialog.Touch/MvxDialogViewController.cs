// MvxDialogViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Touch.Dialog.Elements;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch
{
    public class MvxDialogViewController
        : EventSourceDialogViewController
          , IMvxTouchView
    {
        protected MvxDialogViewController(UITableViewStyle style = UITableViewStyle.Grouped,
                                          RootElement root = null,
                                          bool pushing = false)
            : base(style, root, pushing)
        {
            this.AdaptForBinding();
        }

        public MvxDialogViewController(IntPtr handle) : base(handle)
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
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }

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