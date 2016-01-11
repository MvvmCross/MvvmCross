// IMvxFragmentHost.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.OS;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Attributes;

namespace MvvmCross.Droid.FullFragging.Presenter
{
    public interface IMvxFragmentHost
    {
        bool Show(MvxViewModelRequest request, Bundle bundle, Type fragmentType, MvxFragmentAttribute fragmentAttribute);

        bool Close(IMvxViewModel viewModel);
    }
}