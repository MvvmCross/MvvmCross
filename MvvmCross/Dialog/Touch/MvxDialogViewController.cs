// MvxDialogViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Touch
{
    using System;
    using System.Collections.Generic;

    using CrossUI.Touch.Dialog.Elements;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Touch.Views;

    using UIKit;

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

        public MvxDialogViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return this.DataContext as IMvxViewModel; }
            set { this.DataContext = value; }
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

        #endregion Extra Binding helpers just for Elements
    }

    public class MvxDialogViewController<TViewModel>
        : MvxDialogViewController
          , IMvxTouchView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxDialogViewController(UITableViewStyle style = UITableViewStyle.Grouped,
                                       RootElement root = null,
                                       bool pushing = false)
            : base(style, root, pushing)
        {
        }

        public MvxDialogViewController(IntPtr handle)
            : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}