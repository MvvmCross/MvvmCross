// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform.Platform;

namespace MvvmCross.Platform.Test.Mocks
{
    public class MockBootstrapAction : IMvxBootstrapAction
    {
        public static int CallCount { get; set; }

        public void Run()
        {
            CallCount++;
        }
    }
}