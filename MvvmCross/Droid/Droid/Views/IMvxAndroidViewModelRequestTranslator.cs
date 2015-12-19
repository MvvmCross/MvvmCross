// IMvxAndroidViewModelRequestTranslator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using System;

    using Android.Content;

    using MvvmCross.Core.ViewModels;

    public interface IMvxAndroidViewModelRequestTranslator
    {
        Intent GetIntentFor(MvxViewModelRequest request);

        // Important: if calling GetIntentWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        Tuple<Intent, int> GetIntentWithKeyFor(IMvxViewModel existingViewModelToUse);

        void RemoveSubViewModelWithKey(int key);
    }
}