// MvxConsoleMessagePump.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Console.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleMessagePump : IMvxConsoleCurrentView, IMvxMessagePump
    {
        private readonly MvxConsoleSystemMessageHandler _systemMessageHandler = new MvxConsoleSystemMessageHandler();

        #region IMvxConsoleCurrentView Members

        public IMvxConsoleView CurrentView { get; set; }

        #endregion

        #region IMvxMessagePump Members

        public void Run()
        {
            System.Console.WriteLine("Run loop starting");
            while (!_systemMessageHandler.ExitFlag)
            {
                System.Console.Write(">");
                var input = System.Console.ReadLine();
                if (CurrentView == null)
                {
                    System.Console.WriteLine("Error - no view shown currently");
                    return;
                }

                if (input == null)
                    input = string.Empty;

                if (CurrentView.HandleInput(input))
                    continue;

                if (_systemMessageHandler.HandleInput(CurrentView.ReflectionGetViewModel(), input))
                    continue;

                System.Console.WriteLine("Error - unknown input");
            }
        }

        #endregion
    }
}