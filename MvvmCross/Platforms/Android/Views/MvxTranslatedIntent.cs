// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxTranslatedIntent
    {
        #region TranslationResult enum

        public enum TranslationResult
        {
            Request,
            ExistingViewModel
        }

        #endregion TranslationResult enum

        public MvxTranslatedIntent(MvxViewModelRequest viewModelRequest)
        {
            ViewModelRequest = viewModelRequest;
            Result = TranslationResult.Request;
        }

        public MvxTranslatedIntent(IMvxViewModel existingViewModel)
        {
            ExistingViewModel = existingViewModel;
            Result = TranslationResult.ExistingViewModel;
        }

        public TranslationResult Result { get; private set; }
        public IMvxViewModel ExistingViewModel { get; private set; }
        public MvxViewModelRequest ViewModelRequest { get; private set; }
    }
}
