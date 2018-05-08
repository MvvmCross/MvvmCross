// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Views.Base;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxContentView : MvxEventSourceContentView, IMvxElement
    {
        public MvxContentView()
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
    }

    public class MvxContentView<TViewModel>
        : MvxContentView
    , IMvxElement<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxContentView()
        {
            BindingContextChangedCalled += (sender, args) => ViewModel = base.ViewModel as TViewModel;
        }

        public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel),
            typeof(TViewModel), typeof(MvxContentView<TViewModel>), null, BindingMode.Default, null, null, null, CoerceValue);

        public new TViewModel ViewModel
        {
            get { return (TViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static TViewModel CoerceValue(BindableObject bindable, object value)
        {
            (bindable as MvxContentView<TViewModel>)?.SetBaseViewModel(value as TViewModel);
            return (bindable as MvxContentView<TViewModel>)?.DataContext as TViewModel;
        }

        private void SetBaseViewModel(TViewModel newValue)
        {
            base.ViewModel = newValue;
        }

    }
}
