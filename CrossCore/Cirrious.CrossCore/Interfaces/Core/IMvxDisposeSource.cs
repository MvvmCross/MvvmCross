using System;

namespace Cirrious.CrossCore.Interfaces.Core
{
    public interface IMvxDisposeSource
    {
        event EventHandler DisposeCalled;
    }
}