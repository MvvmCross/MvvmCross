using System;

namespace Phone7.Fx.Messaging
{
    public interface IDispatcherFacade
    {
        void BeginInvoke(Delegate method, object arg);
    }
}