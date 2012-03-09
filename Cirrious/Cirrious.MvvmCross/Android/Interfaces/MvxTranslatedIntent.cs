#region Copyright
// <copyright file="MvxTranslatedIntent.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Android.Interfaces
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