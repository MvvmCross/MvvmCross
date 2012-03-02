#region Copyright
// <copyright file="MvxPhonePage.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Linq;
using System.Windows.Navigation;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public abstract class MvxPhonePage<T>
        : PhoneApplicationPage
        , IMvxWindowsPhoneView<T>
        where T : class, IMvxViewModel
    {
        private T _viewModel;

        #region IMvxWindowsPhoneView<T> Members

        public bool IsVisible { get; set; }

        public T ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel == value)
                    return;

                _viewModel = value;
                DataContext = ViewModel;
            }
        }

        public void ClearBackStack()
        {
            // note - we do *not* use CanGoBack here - as that seems to always returns true!
            while (NavigationService.BackStack.Any())
                NavigationService.RemoveBackEntry();
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            IsVisible = true;

#warning I'm not 100% happy with the use of created and destroyed here - cross platform code - huh?
            this.OnViewCreate(e.Uri);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            IsVisible = false;
            base.OnNavigatedFrom(e);

#warning I'm not 100% happy with the use of created and destroyed here - cross platform code - huh?
            if (e.NavigationMode == NavigationMode.Back)
                this.OnViewDestroy();
        }
    }
}
