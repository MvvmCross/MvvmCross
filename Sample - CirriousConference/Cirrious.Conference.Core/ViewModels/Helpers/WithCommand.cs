using System.Windows.Input;
using System;

namespace Cirrious.Conference.Core.ViewModels.Helpers
{
    public class WithCommand<T> : IDisposable
    {
        public WithCommand(T item, ICommand command)
        {
            Command = command;
            Item = item;
        }

        public T Item { get; private set; }
        public ICommand Command { get; private set; }

		#region IDisposable implementation
		
		public void Dispose ()
		{
			this.Dispose(true);
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				var disposableCommand = Command as IDisposable;
				if (disposableCommand != null)
				{
					disposableCommand.Dispose();
				}
				Command = null;
			}
		}
		
		#endregion
    }
}