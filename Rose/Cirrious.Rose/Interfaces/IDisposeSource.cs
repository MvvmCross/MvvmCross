using System;

namespace Cirrious.MvvmCross.Droid.Views
{
    public interface IDisposeSource
    {
        event EventHandler DisposeCalled;
    }
}