#region Copyright
// <copyright file="MvxPhonePage.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Windows.Navigation;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public abstract class MvxPhonePage<T>
        : PhoneApplicationPage
          , IMvxView
          , IMvxServiceConsumer<IMvxWindowsPhoneViewModelRequestTranslator>
          , IMvxServiceConsumer<IMvxViewModelLoader> 
          where T : IMvxViewModel
    {
        public T ViewModel { get; set; }

        #region IMvxView Members

        public Type ViewModelType
        {
            get { return typeof (T); }
        }

        public void SetViewModel(object model)
        {
            ViewModel = (T) model;
            DataContext = ViewModel;
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var translatorService = this.GetService<IMvxWindowsPhoneViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetRequestFromXamlUri(e.Uri);

            var loaderService = this.GetService<IMvxViewModelLoader>();
            var model = loaderService.LoadModel(viewModelRequest);

            SetViewModel(model);
        }
    }
}
