using System;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public interface IMvxTimeListenerTarget
    {
        void InternalSetValueAndRaiseChanged(TimeSpan time);
    }
}