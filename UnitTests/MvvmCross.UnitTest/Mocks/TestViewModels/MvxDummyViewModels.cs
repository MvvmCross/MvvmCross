// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Mocks.TestViewModels
{
    public class ViewModelA
        : MvxViewModel
    {
    }

    public class ViewModelB
        : MvxViewModel
    {
    }

    public class ViewModelC
        : MvxViewModel
    {
        public void Init(string id)
        {
        }
    }

    public class ViewModelD
        : MvxViewModel
    {
    }
}
