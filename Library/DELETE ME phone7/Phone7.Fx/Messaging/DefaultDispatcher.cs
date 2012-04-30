using System;
using System.Windows;

namespace Phone7.Fx.Messaging
{
    public class DefaultDispatcher : IDispatcherFacade
    {
        /// <summary>
        /// Forwards the BeginInvoke to the current deployment's <see cref="System.Windows.Threading.Dispatcher"/>.
        /// </summary>
        /// <param name="method">Method to be invoked.</param>
        /// <param name="arg">Arguments to pass to the invoked method.</param>
        public void BeginInvoke(Delegate method, object arg)
        {
            if (Deployment.Current != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(method, arg);
            }
        }
    }
}