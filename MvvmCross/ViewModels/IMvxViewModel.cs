// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

namespace MvvmCross.ViewModels
{
    public interface IMvxViewModel
    {
        void ViewCreated();

        void ViewAppearing();

        void ViewAppeared();

        void ViewDisappearing();

        void ViewDisappeared();

        void ViewDestroy(bool viewFinishing = true);

        void Init(IMvxBundle parameters);

        void ReloadState(IMvxBundle state);

        void Start();

        void SaveState(IMvxBundle state);

        void Prepare();

        Task Initialize();

        MvxNotifyTask? InitializeTask { get; set; }
    }

    public interface IMvxViewModel<in TParameter>
        : IMvxViewModel
    {
        void Prepare(TParameter parameter);
    }
}
