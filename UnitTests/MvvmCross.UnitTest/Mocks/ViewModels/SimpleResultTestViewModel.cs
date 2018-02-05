// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Test.Mocks.ViewModels
{
    public class SimpleResultTestViewModel : MvxViewModelResult<bool>
    {
        public SimpleResultTestViewModel()
        {
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            await Task.Delay(2000);
            CloseCompletionSource.SetResult(true);
        }
    }
}
