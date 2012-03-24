using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.Conference.Core.ViewModels.Helpers
{
    public class WithCommand<T>
    {
        public WithCommand(T item, IMvxCommand command)
        {
            Command = command;
            Item = item;
        }

        public T Item { get; private set; }
        public IMvxCommand Command { get; private set; }
    }
}