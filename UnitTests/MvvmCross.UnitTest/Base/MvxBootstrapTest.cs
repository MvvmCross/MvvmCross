// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.UnitTest.Base.Mocks;
using Xunit;

namespace MvvmCross.UnitTest.Base
{

    public class MvxBootstrapTest
    {
        [Fact]
        public void Test_Bootstrap_Calls_Our_Mock()
        {
            MockBootstrapAction.CallCount = 0;
            var runner = new MvxBootstrapRunner();
            runner.Run(GetType().Assembly);
            Assert.Equal(1, MockBootstrapAction.CallCount);
        }
    }
}
