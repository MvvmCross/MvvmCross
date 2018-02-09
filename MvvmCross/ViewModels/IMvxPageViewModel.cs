// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels
{
    public interface IMvxPagedViewModel : IMvxViewModel
    {
        string PagedViewId { get; }
    }

    public interface IMvxPageViewModel : IMvxViewModel
    {
        IMvxPagedViewModel GetDefaultViewModel();

        IMvxPagedViewModel GetNextViewModel(IMvxPagedViewModel currentViewModel);

        IMvxPagedViewModel GetPreviousViewModel(IMvxPagedViewModel currentViewModel);
    }
}