// MvxBootstrapTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Test
{
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Test.Mocks;

    using NUnit.Framework;

    [TestFixture]
    public class MvxBootstrapTest
    {
        [Test]
        public void Test_Bootstrap_Calls_Our_Mock()
        {
            MockBootstrapAction.CallCount = 0;
            var runner = new MvxBootstrapRunner();
            runner.Run(this.GetType().Assembly);
            Assert.AreEqual(1, MockBootstrapAction.CallCount);
        }
    }
}