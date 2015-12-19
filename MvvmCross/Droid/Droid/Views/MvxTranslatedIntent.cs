// MvxTranslatedIntent.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using MvvmCross.Core.ViewModels;

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
            this.ViewModelRequest = viewModelRequest;
            this.Result = TranslationResult.Request;
        }

        public MvxTranslatedIntent(IMvxViewModel existingViewModel)
        {
            this.ExistingViewModel = existingViewModel;
            this.Result = TranslationResult.ExistingViewModel;
        }

        public TranslationResult Result { get; private set; }
        public IMvxViewModel ExistingViewModel { get; private set; }
        public MvxViewModelRequest ViewModelRequest { get; private set; }
    }
}