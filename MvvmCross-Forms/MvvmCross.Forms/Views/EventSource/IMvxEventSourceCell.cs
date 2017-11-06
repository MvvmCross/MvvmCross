using System;
namespace MvvmCross.Forms.Views.EventSource
{
    public interface IMvxEventSourceCell : IMvxEventSourceElement
    {
        event EventHandler AppearingCalled;

        event EventHandler DisappearingCalled;

        event EventHandler TappedCalled;
    }
}
