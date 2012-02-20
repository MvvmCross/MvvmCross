using System;

namespace Cirrious.MvvmCross.Core
{
    public abstract class MvxAsyncDispatcher
    {
		protected void BeginAsync(Action action)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(ignored => action());
        }
		
		protected void BeginAsync(Action<object> action, object state)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(arg => action(arg), state);
        }
    }
}