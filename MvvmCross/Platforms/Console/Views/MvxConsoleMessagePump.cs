﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platforms.Console.Core;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Console.Views
{
#nullable enable
    public class MvxConsoleMessagePump : IMvxConsoleCurrentView, IMvxMessagePump
    {
        private readonly MvxConsoleSystemMessageHandler _systemMessageHandler = new MvxConsoleSystemMessageHandler();

        public IMvxConsoleView? CurrentView { get; set; }

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
    }
#nullable restore
}
