using System;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public interface IMvxDateListenerTarget
    {
        void InternalSetValueAndRaiseChanged(DateTime date);
    }
}