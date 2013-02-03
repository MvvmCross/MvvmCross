// MvxBindingTouchViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
	public class MvxBindingViewControllerAdapter : BaseViewControllerAdapter
	{
		protected IMvxBindingTouchView BindingTouchView {
			get {
				return TouchView as IMvxBindingTouchView;
			}
		}

		public MvxBindingViewControllerAdapter (IMvxBindingTouchView view)
			: base (view as IViewControllerEventSource)
		{			
		}

		public override void HandleIsDisposingDisposeCalled (object sender, EventArgs e)
		{
			BindingTouchView.ClearBindings();
			base.HandleIsDisposingDisposeCalled (sender, e);
		}
	}

	public class MvxBindingViewController
		: MvxViewController
		, IMvxBindingTouchView
	{
		protected MvxBindingViewController()
			: base()
		{
			var adapter = new MvxBindingViewControllerAdapter(this);
		}
		
		protected MvxBindingViewController(string nibName, NSBundle bundle)
			: base(nibName, bundle)
		{
			var adapter = new MvxBindingViewControllerAdapter(this);
		}

		private readonly List<IMvxUpdateableBinding> _bindings = new List<IMvxUpdateableBinding>();
		
		public List<IMvxUpdateableBinding> Bindings
		{
			get { return _bindings; }
		}
	}

    public class MvxBindingTouchViewController<TViewModel>
        : MvxTouchViewController<TViewModel>
        , IMvxBindingTouchView
        where TViewModel : class, IMvxViewModel
    {
        protected MvxBindingTouchViewController(MvxShowViewModelRequest request)
            : base(request)
        {
        }

        protected MvxBindingTouchViewController(MvxShowViewModelRequest request, string nibName, NSBundle bundle)
            : base(request, nibName, bundle)
        {
        }

        #region Shared area needed by all binding controllers

        private readonly List<IMvxUpdateableBinding> _bindings = new List<IMvxUpdateableBinding>();

        public List<IMvxUpdateableBinding> Bindings
        {
            get { return _bindings; }
        }

        public virtual object DefaultBindingSource
        {
            get { return ViewModel; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearBindings();
            }

            base.Dispose(disposing);
        }

#warning really need to think about how to handle ios6 once ViewDidUnload has been removed
        [Obsolete]
        public override void ViewDidUnload()
        {
            this.ClearBindings();
            base.ViewDidUnload();
        }

        #endregion
    }
}