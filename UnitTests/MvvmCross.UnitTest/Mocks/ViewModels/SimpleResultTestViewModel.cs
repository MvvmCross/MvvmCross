// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Mocks.ViewModels
{
    public class SimpleResult
    {
        public bool Result { get; set; }
    }

    public class SimpleResultTestViewModel : MvxViewModelResult<SimpleResult>
    {
        public SimpleResultTestViewModel()
        {
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            await Task.Delay(2000);
            CloseCompletionSource.SetResult(new SimpleResult { Result = true });
        }
    }
}
