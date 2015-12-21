// IMvxFragmentHost.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Cirrious.MvvmCross.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using System;

namespace MvvmCross.Droid.Support.V7.Fragging.Presenter
{
    public interface IMvxFragmentHost
    {
        bool Show(MvxViewModelRequest request, Bundle bundle, Type fragmentType, MvxFragmentAttribute fragmentAttribute);

        bool Close(IMvxViewModel viewModel);
    }
}