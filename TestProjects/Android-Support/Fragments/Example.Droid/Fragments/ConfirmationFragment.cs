using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Example.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views.Attributes;

namespace Example.Droid.Fragments
{
    [MvxDialogFragmentPresentation(Cancelable = true)]
    [Register(nameof(ConfirmationFragment))]
    public class ConfirmationFragment : MvxAppCompatDialogFragment<ConfirmationViewModel>
    {
        public ConfirmationFragment()
        {
            RetainInstance = true;
        }

        public ConfirmationFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            RetainInstance = true;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var builder = new AlertDialog.Builder(Activity)
                .SetTitle(ViewModel.Title)
                .SetMessage(ViewModel.Body)
                .SetPositiveButton(ViewModel.PositiveCommandText, OnPositiveButton)
                .SetNegativeButton(ViewModel.NegativeCommandText, OnNegativeButton);
            return builder.Create();
        }

        private async void OnNegativeButton(object sender, DialogClickEventArgs e)
        {
            if (ViewModel.NegativeCommand.CanExecute())
            {
                await ViewModel.NegativeCommand.ExecuteAsync();
            }
        }

        private async void OnPositiveButton(object sender, DialogClickEventArgs e)
        {
            if (ViewModel.PositiveCommand.CanExecute())
            {
                await ViewModel.PositiveCommand.ExecuteAsync();
            }
        }
    }
}
