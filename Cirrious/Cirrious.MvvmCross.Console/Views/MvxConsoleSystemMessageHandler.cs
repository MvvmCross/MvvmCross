#region Copyright

// <copyright file="MvxConsoleSystemMessageHandler.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleSystemMessageHandler
        : IMvxServiceConsumer<IMvxViewDispatcherProvider>
    {
        public bool ExitFlag { get; set; }

        private IMvxViewDispatcher ViewDispatcher
        {
            get { return this.GetService().Dispatcher; }
        }

        public virtual bool HandleInput(IMvxViewModel viewModel, string input)
        {
            input = input.ToUpper();
            switch (input)
            {
                case "BACK":
                case "B":
                    ViewDispatcher.RequestClose(viewModel);
                    return true;
                case "QUIT":
                case "Q":
                    ExitFlag = true;
                    return true;
            }

            return false;
        }
    }
}