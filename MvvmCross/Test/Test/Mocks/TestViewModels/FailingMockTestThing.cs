// FailingMockTestThing.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Mocks.TestViewModels
{
    using System;

    public class FailingMockTestThing : ITestThing
    {
        public FailingMockTestThing()
        {
            throw new Exception("I always fail");
        }
    }
}