using Playground.Uwp;

namespace Playground.Uno.Wasm
{
    public class Program
    {
        private static App _app;

        private static int Main(string[] args)
        {
            Windows.UI.Xaml.Application.Start(_ => _app = new App());

            return 0;
        }
    }
}
