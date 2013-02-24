﻿using System.Linq;
using System.Text;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Console.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace TwitterSearch.UI.Console
{
    class Program
        : IMvxConsumer
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
