using MvvmCross.Platform.Mac.Core;
using MvvmCross.Platform.Mac.Views.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core;

namespace Playground.Mac
{
    public class Setup : MvxMacSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate) : base(applicationDelegate)
        {
            MvxWindowPresentationAttribute.DefaultWidth = 250;
            MvxWindowPresentationAttribute.DefaultHeight = 250;
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }
    }
}
