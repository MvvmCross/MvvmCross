using System;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMainThreadDispatcher
    {
        bool RequestMainThreadAction(Action action);
    }
}