using System;

namespace DroidAutoComplete
{
    public interface IMainThreadRunner
    {
        void Run(Action action);
    }
}