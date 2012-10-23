using System;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;

namespace CustomerManagement.Droid.Views
{
    public class MvxDefaultViewConstants
    {
        public const string MvxBindTag = "MvxBind";
        public const string Dialog = "Dialog";
        public const string Menu = "Menu";
    }

    public interface IMvxBindingDialogActivity : IMvxBindingActivity
    {
        void RegisterDialogBinding(IDisposable disposable);
    }
}