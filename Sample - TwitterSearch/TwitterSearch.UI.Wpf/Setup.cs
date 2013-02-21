using System.Windows.Threading;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Wpf.Interfaces;
using Cirrious.MvvmCross.Wpf.Platform;
using TwitterSearch.Core;

namespace TwitterSearch.UI.Wpf
{
    public class Setup
        : MvxWpfSetup
    {
        public Setup(Dispatcher dispatcher, IMvxWpfViewPresenter presenter)
            : base(dispatcher, presenter)
        {
        }

        protected override MvxApplication CreateApp()
        {
            return new TwitterSearchApp();
        }
    }
}
