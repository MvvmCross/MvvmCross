using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvxMainThread;

public class ToBeWoven : MvxMainThreadDispatchingObject
{
    [RunOnMainThread]
    public void SomeWovenMainThreadMethod()
    {
        Console.WriteLine($"This is running on the main thread");
    }
}

public class ToBeWovenNonDispatching
{
    [RunOnMainThread]
    public void SomeWovenMainThreadMethod()
    {
        Console.WriteLine($"This is running on the main thread");
    }
}

public class NotWovenNonDispatching
{
    public void SomeWovenMainThreadMethod()
    {
        var dispatcher = Mvx.Resolve<IMvxMainThreadDispatcher>();
        dispatcher?.RequestMainThreadAction(_SomeWovenMainThreadMethod_Woven);
    }

    private void _SomeWovenMainThreadMethod_Woven()
    {
        Console.WriteLine($"This is running on the main thread");
    }
}
