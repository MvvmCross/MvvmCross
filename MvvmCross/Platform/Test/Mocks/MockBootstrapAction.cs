// MockBootstrapAction.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Test.Mocks
{
    using MvvmCross.Platform.Platform;

    public class MockBootstrapAction : IMvxBootstrapAction
    {
        public static int CallCount { get; set; }

        public void Run()
        {
            CallCount++;
        }
    }
}