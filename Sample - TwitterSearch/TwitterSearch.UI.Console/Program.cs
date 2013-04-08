using System.Linq;
using System.Text;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Console.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace TwitterSearch.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        void Run()
        {
            // initialize app
            var setup = new Setup();
            setup.Initialize();

            // trigger the first navigate...
            var starter = Mvx.Resolve<IMvxAppStart>();
            starter.Start();

            // enter the run loop
            var pump = Mvx.Resolve<IMvxMessagePump>();
            pump.Run();
        }
    }
}
