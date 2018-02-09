// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;

namespace MvvmCross.Forms.Views
{
    public interface IMvxPage : IMvxElement
    {
    }

    public interface IMvxPage<TViewModel>
        : IMvxPage, IMvxElement<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
