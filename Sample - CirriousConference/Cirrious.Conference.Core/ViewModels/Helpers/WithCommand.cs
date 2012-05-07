using System.Windows.Input;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.Conference.Core.ViewModels.Helpers
{
    public class WithCommand<T>
    {
        public WithCommand(T item, ICommand command)
        {
            Command = command;
            Item = item;
        }

        public T Item { get; private set; }
        public ICommand Command { get; private set; }
    }
}