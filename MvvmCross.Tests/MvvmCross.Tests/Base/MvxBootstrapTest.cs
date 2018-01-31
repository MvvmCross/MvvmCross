// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Test.Mocks;
using NUnit.Framework;

namespace MvvmCross.Platform.Test
{
    [TestFixture]
    public class MvxBootstrapTest
    {
        [Test]
        public void Test_Bootstrap_Calls_Our_Mock()
        {
            MockBootstrapAction.CallCount = 0;
            var runner = new MvxBootstrapRunner();
            runner.Run(GetType().Assembly);
            Assert.AreEqual(1, MockBootstrapAction.CallCount);
        }
    }
}