// IMvxIosViewCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Views
{
    public interface IMvxIosViewCreator : IMvxCurrentRequest
    {
        IMvxIosView CreateView(MvxViewModelRequest request);

        IMvxIosView CreateView(IMvxViewModel viewModel);

        IMvxIosView CreateViewOfType(Type viewType, MvxViewModelRequest request);
    }
}