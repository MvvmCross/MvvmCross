using System;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;

namespace CustomerManagement.Droid.Views
{
    public interface IMvxBindingDialogActivity : IMvxBindingActivity
    {
        void RegisterDialogBinding(IDisposable disposable);
    }
}