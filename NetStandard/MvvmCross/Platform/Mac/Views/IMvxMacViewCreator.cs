// IMvxMacViewCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Mac.Views
{
    public interface IMvxMacViewCreator
    {
        IMvxMacView CreateView(MvxViewModelRequest request);

        IMvxMacView CreateView(IMvxViewModel viewModel);

        IMvxMacView CreateViewOfType(Type viewType, MvxViewModelRequest request);
    }
}