// IMvxAndroidViewModelLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Droid.Views
{
    public interface IMvxAndroidViewModelLoader
    {
        IMvxViewModel Load(Intent intent, IMvxBundle savedState);

        IMvxViewModel Load(Intent intent, IMvxBundle savedState, Type viewModelTypeHint);
    }
}