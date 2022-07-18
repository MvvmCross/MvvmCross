// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views
{
    public interface IMvxMultipleViewModelCache
    {
        void Cache(IMvxViewModel toCache, string viewModelTag = "singleInstanceCache");

        IMvxViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache");

        T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : IMvxViewModel;
    }
}
