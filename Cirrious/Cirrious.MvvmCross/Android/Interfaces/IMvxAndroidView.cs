#region Copyright

// <copyright file="IMvxAndroidView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Android.Interfaces
{
    public interface IMvxAndroidView<TViewModel>
        : IMvxView
          , IMvxServiceConsumer<IMvxViewModelLoader>
          , IMvxServiceConsumer<IMvxAndroidViewModelRequestTranslator>
          , IMvxServiceConsumer<IMvxAndroidActivityTracker>
          , IMvxServiceConsumer<IMvxAndroidSubViewServices>
        where TViewModel : class, IMvxViewModel
    {
        TViewModel ViewModel { get; set; }
        bool IsSubView { get; }
    }
}