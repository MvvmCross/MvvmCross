using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Pss.Views
{
    public class MvxPssSystemMessageHandler
        : IMvxServiceConsumer
    {
        public bool ExitFlag { get; set; }

		private IMvxViewDispatcher ViewDispatcher { get { return this.GetService<IMvxViewDispatcherProvider>().Dispatcher; } }

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