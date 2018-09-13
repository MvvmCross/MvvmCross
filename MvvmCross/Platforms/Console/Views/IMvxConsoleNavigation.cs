// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Presenters;

namespace MvvmCross.Platforms.Console.Views
{
    public interface IMvxConsoleNavigation
        : IMvxViewPresenter
    {
        Task<bool> GoBack();

        void RemoveBackEntry();

        bool CanGoBack();
    }
}
