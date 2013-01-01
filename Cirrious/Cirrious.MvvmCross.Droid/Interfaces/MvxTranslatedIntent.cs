// MvxTranslatedIntent.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Interfaces
{
    public class MvxTranslatedIntent
    {
        #region TranslationResult enum

        public enum TranslationResult
        {
            Request,
            ExistingViewModel
        }

        #endregion

        public MvxTranslatedIntent(MvxShowViewModelRequest showViewModelRequest)
        {
            ShowViewModelRequest = showViewModelRequest;
            Result = TranslationResult.Request;
        }

        public MvxTranslatedIntent(IMvxViewModel existingViewModel)
        {
            ExistingViewModel = existingViewModel;
            Result = TranslationResult.ExistingViewModel;
        }

        public TranslationResult Result { get; private set; }
        public IMvxViewModel ExistingViewModel { get; private set; }
        public MvxShowViewModelRequest ShowViewModelRequest { get; private set; }
    }
}