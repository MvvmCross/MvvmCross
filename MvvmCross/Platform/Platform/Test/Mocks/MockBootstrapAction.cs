// MockBootstrapAction.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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