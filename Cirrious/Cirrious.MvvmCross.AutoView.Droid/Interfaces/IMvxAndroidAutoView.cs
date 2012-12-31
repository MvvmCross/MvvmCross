#region Copyright

// <copyright file="IMvxAndroidAutoView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.AutoView.Droid.Interfaces
{
    public interface IMvxAndroidAutoView<TViewModel>
        : IMvxAndroidView<TViewModel>
          , IMvxAutoView
          , IMvxBindingActivity
        where TViewModel : class, IMvxViewModel
    {
    }
}