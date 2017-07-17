using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Mac.Platform;
using MvvmCross.Mac.Views.Presenters;
using Playground.Core;

namespace Playground.Mac
{
    public class Setup : MvxMacSetup
    {
        public Setup(MvxApplicationDelegate appDelegate, IMvxMacViewPresenter presenter)
            : base(appDelegate, presenter)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }
    }
}
