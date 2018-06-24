// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Console.Views
{
    public class MvxConsoleSystemMessageHandler
    {
        public bool ExitFlag { get; set; }

        private IMvxConsoleNavigation ConsoleNavigation => Mvx.IoCProvider.Resolve<IMvxConsoleNavigation>();

        public virtual bool HandleInput(IMvxViewModel viewModel, string input)
        {
            input = input.ToUpper();
            switch (input)
            {
                case "BACK":
                case "B":
                    if (ConsoleNavigation.CanGoBack())
                        ConsoleNavigation.GoBack();
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
