// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using MvvmCross.Platforms.Console.Core;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Console.Views;

public class MvxConsoleMessagePump : IMvxConsoleCurrentView, IMvxMessagePump
{
    private readonly MvxConsoleSystemMessageHandler _systemMessageHandler = new();

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

            if (_systemMessageHandler.HandleInput(ReflectionGetViewModel(CurrentView), input))
                continue;

            System.Console.WriteLine("Error - unknown input");
        }
    }

    private static IMvxViewModel? ReflectionGetViewModel(IMvxView? view)
    {
        var propertyInfo = view?.GetType().GetProperty("ViewModel");
        return (IMvxViewModel?)propertyInfo?.GetGetMethod()?.Invoke(view, new object[] { });
    }
}
