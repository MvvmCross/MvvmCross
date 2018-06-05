// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Views.Base;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxContentPage : MvxEventSourceContentPage, IMvxPage
    {
        public MvxContentPage()
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get
            {
                return BindingContext.DataContext;
            }
            set
            {
                base.BindingContext = value;
                BindingContext.DataContext = value;
            }
        }

        private IMvxBindingContext _bindingContext;
        public new IMvxBindingContext BindingContext
        {
            get
            {
                if (_bindingContext == null)
                    BindingContext = new MvxBindingContext(base.BindingContext);
                return _bindingContext;
            }
            set
            {
                _bindingContext = value;
            }
        }

        public IMvxViewModel ViewModel
        {
            get
            {
                return DataContext as IMvxViewModel;
            }
            set 
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

		protected virtual void OnViewModelSet()
		{
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.ViewAppearing();
            ViewModel?.ViewAppeared();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel?.ViewDisappearing();
            ViewModel?.ViewDisappeared();
        }
    }

    public class MvxContentPage<TViewModel>
        : MvxContentPage
    , IMvxPage<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxContentPage()
        {
            BindingContextChangedCalled += SetViewModelProperty;
        }

        ~MvxContentPage()
        {
            BindingContextChangedCalled -= SetViewModelProperty;
        }

        public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel),
            typeof(TViewModel), typeof(MvxContentPage<TViewModel>), null, BindingMode.Default, null, ViewModelBindingPropertyChanged, null);

        public new TViewModel ViewModel
        {
            get { return base.ViewModel as TViewModel; }
            set { base.ViewModel = value; }
        }

        //Necessary to set ViewModelProperty when ViewModel/DataContext/BindingContext is set
        private void SetViewModelProperty(object sender, EventArgs args)
        {
            //Prevents ViewModelProperty from getting redundantly set a second time after
            //setting ViewModel in ViewModelBindingPropertyChanged
            if (GetValue(ViewModelProperty) != ViewModel)
            {
                SetValue(ViewModelProperty, ViewModel);
            }
        }

        //Necessary to set base.viewmodel when ViewModelProperty is set through binding
        private static void ViewModelBindingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //Prevents ViewModel from getting redundantly set a second time after
            //setting ViewModelProperty on BindingContextChangedCalled
            if (((MvxContentPage<TViewModel>) bindable).ViewModel != newValue)
            {
                ((MvxContentPage<TViewModel>) bindable).ViewModel = newValue as TViewModel;
            }
        }
    }
}
