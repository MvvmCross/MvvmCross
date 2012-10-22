using System;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;

namespace CustomerManagement.Droid.Views
{
    public class MvxDefaultViewConstants
    {
        public const string Dialog = "Dialog";
    }

    public interface IMvxBindingDialogActivity : IMvxBindingActivity
    {
        void RegisterDialogBinding(IDisposable disposable);
    }
}