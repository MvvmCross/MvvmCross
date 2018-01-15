// IMvxTvosViewCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.tvOS.Views
{
    public interface IMvxTvosViewCreator : IMvxCurrentRequest
    {
        IMvxTvosView CreateView(MvxViewModelRequest request);

        IMvxTvosView CreateView(IMvxViewModel viewModel);

        IMvxTvosView CreateViewOfType(Type viewType, MvxViewModelRequest request);
    }
}