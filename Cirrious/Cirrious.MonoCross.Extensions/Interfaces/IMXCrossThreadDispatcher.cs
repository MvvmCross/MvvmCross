using System;

namespace Cirrious.MonoCross.Extensions.Interfaces
{
    public interface IMXCrossThreadDispatcher
    {
        bool RequestMainThreadAction(Action action);
    }
}