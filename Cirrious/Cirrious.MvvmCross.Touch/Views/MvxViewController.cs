using System.Collections.Generic;
using Cirrious.CrossCore.Touch;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxViewController 
        : MvxEventSourceViewController
          , IMvxBindingTouchView
    {
        public MvxViewController ()
        {
            this.AdaptForBinding();
        }

        protected MvxViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public virtual object DataContext { get;set; }
		
        public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel)DataContext; }
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