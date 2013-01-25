using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Cirrious.MvvmCross.Console.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using CustomerManagement;

namespace CustomerManagement.Console
{
    class Program
        : IMvxServiceConsumer
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
            var starter = this.GetService<IMvxStartNavigation>();
            starter.Start();

            // enter the run loop
            var pump = this.GetService<IMvxMessagePump>();
            pump.Run();
        }
    }
}
