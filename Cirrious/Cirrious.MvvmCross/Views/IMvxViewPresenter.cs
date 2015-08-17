// IMvxViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Views
{
    public interface IMvxViewPresenter
    {
        void Show(MvxViewModelRequest request);
        void ChangePresentation(MvxPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, bool> action) where THint : MvxPresentationHint;
    }
}