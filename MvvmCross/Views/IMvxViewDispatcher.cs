// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;

namespace MvvmCross.Views
{
    public interface IMvxViewDispatcher : IMvxMainThreadAsyncDispatcher, IMvxMainThreadDispatcher
    {
        IMvxViewPresenter Presenter { get; set; }
        Task<bool> ShowViewModel(MvxViewModelRequest request);

        Task<bool> ChangePresentation(MvxPresentationHint hint);
    }
}
