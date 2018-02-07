// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.UnitTest.Mocks.TestViewModels
{
    public class Test4ViewModel : MvxViewModel
    {
        public ITestThing Thing { get; private set; }

        public Test4ViewModel(ITestThing thing)
        {
            Thing = thing;
        }

        public override void Start()
        {
            throw new NullReferenceException();
        }
    }
}
