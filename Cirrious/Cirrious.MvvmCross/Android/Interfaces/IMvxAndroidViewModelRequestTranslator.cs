#region Copyright
// <copyright file="IMvxAndroidViewModelRequestTranslator.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Android.Content;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Android.Interfaces
{
    public interface IMvxAndroidViewModelLoader
    {
        IMvxViewModel Load(Intent intent);
        IMvxViewModel Load(Intent intent, Type viewModelTypeHint);
    }

    public interface IMvxAndroidViewModelRequestTranslator
    {
        Intent GetIntentFor(MvxShowViewModelRequest request);

        // Important: if calling GetIntentWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        Tuple<Intent, int> GetIntentWithKeyFor(IMvxViewModel existingViewModelToUse);
        void RemoveSubViewModelWithKey(int key);
    }
}