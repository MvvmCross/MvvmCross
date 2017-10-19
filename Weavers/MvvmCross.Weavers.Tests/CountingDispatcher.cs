using System;
using MvvmCross.Platform.Core;

public class CountingDispatcher : MvxSingleton<IMvxMainThreadDispatcher>, IMvxMainThreadDispatcher
{
    public int Calls { get; private set; }

    public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
    {
        Calls++;
        action();
        return true;
    }
}