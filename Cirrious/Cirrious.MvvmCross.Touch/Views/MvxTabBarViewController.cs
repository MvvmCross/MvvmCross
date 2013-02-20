using System.Collections.Generic;
using Cirrious.CrossCore.Touch;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTabBarViewController
        : EventSourceTabBarController
          , IMvxBindingTouchView
    {
        protected MvxTabBarViewController()
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