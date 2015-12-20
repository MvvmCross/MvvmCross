// MvxConsoleMessagePump.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Console.Views
{
    using MvvmCross.Console.Platform;
    using MvvmCross.Core.Views;

    public class MvxConsoleMessagePump : IMvxConsoleCurrentView, IMvxMessagePump
    {
        private readonly MvxConsoleSystemMessageHandler _systemMessageHandler = new MvxConsoleSystemMessageHandler();

        #region IMvxConsoleCurrentView Members

        public IMvxConsoleView CurrentView { get; set; }

        #endregion IMvxConsoleCurrentView Members

        #region IMvxMessagePump Members

        public void Run()
        {
            System.Console.WriteLine("Run loop starting");
            while (!this._systemMessageHandler.ExitFlag)
            {
                System.Console.Write(">");
                var input = System.Console.ReadLine();
                if (this.CurrentView == null)
                {
                    System.Console.WriteLine("Error - no view shown currently");
                    return;
                }

                if (input == null)
                    input = string.Empty;

                if (this.CurrentView.HandleInput(input))
                    continue;

                if (this._systemMessageHandler.HandleInput(this.CurrentView.ReflectionGetViewModel(), input))
                    continue;

                System.Console.WriteLine("Error - unknown input");
            }
        }

        #endregion IMvxMessagePump Members
    }
}