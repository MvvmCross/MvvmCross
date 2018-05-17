using System.Threading.Tasks;
using MvvmCross.Commands;
using Playground.Droid.ViewModels;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace StarWarsSample.Droid.ViewModels
{
    public class SecondViewModel : BaseViewModel
    {
        public SecondViewModel()
        {
            OpenBrowserCommand = new MvxAsyncCommand(OpenBrowser);
        }

        public override Task Initialize()
        {
            return Task.FromResult(0);
        }

        public IMvxCommand OpenBrowserCommand { get; private set; }

        private async Task OpenBrowser()
        {
            await CrossShare.Current.OpenBrowser("https://www.google.com", new BrowserOptions { UseSafariWebViewController = false });
        }
    }
}
