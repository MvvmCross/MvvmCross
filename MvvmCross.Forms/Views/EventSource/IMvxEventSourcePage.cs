using System;
namespace MvvmCross.Forms.Views.EventSource
{
    public interface IMvxEventSourcePage : IMvxEventSourceElement
    {
        event EventHandler AppearingCalled;

        event EventHandler DisappearingCalled;
    }
}