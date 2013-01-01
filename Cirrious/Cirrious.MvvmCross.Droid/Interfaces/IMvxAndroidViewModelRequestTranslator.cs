// IMvxAndroidViewModelRequestTranslator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Interfaces
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