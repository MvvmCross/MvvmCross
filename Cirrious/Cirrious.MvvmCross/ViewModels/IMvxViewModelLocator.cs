// IMvxViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.ViewModels
{
    public interface IMvxViewModelLocator
    {
        IMvxViewModel Load(Type viewModelType, IMvxBundle parameterValues, IMvxBundle savedState);

        IMvxViewModel Reload(IMvxViewModel viewModel, IMvxBundle parameterValues, IMvxBundle savedState);
    }
}