using System;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Console.Platform;
using Cirrious.MvvmCross.Console.Views;
using Cirrious.MvvmCross.Interfaces.Views;
using Moq;
using TwitterSearch.Core.Interfaces;

namespace TwitterSearch.Test.Mocks
{
    public class MockSetup
        : MvxBaseConsoleSetup
    {
        private readonly Mock<ITwitterSearchProvider> _mockTwitter = new Mock<ITwitterSearchProvider>();
        private readonly Mock<MvxBaseConsoleContainer> _mockContainer = new Mock<MvxBaseConsoleContainer>();
        private readonly Mock<IMvxViewDispatcher> _mockDispatcher = new Mock<IMvxViewDispatcher>();
        
        public Mock<ITwitterSearchProvider> Twitter { get { return _mockTwitter; } }
        public Mock<MvxBaseConsoleContainer> Container { get { return _mockContainer; } }
        public Mock<IMvxViewDispatcher> Dispatcher { get { return _mockDispatcher; } }

        public MockSetup()
        {
            _mockDispatcher
                .Setup(x => x.RequestMainThreadAction(It.IsAny<Action>()))
                .Callback<Action>((action) => action());

            _mockContainer
                .SetupGet(x => x.Dispatcher)
                .Returns(_mockDispatcher.Object);
        }

        protected override MvxApplication CreateApp()
        {
            return new MockApp(_mockTwitter.Object);
        }

        protected override MvxBaseConsoleContainer CreateConsoleContainer()
        {
            return _mockContainer.Object;
        }
    }
}