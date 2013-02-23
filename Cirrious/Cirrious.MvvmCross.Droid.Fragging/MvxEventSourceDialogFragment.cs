using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Interfaces.Core;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public class MvxEventSourceDialogFragment
        : DialogFragment
          , IMvxEventSourceFragment
    {
        public event EventHandler DisposeCalled;
        public event EventHandler<MvxValueEventArgs<MvxCreateViewParameters>> OnCreateViewCalled;
        public event EventHandler OnDestroyViewCalled;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            OnCreateViewCalled.Raise(this, new MvxCreateViewParameters(inflater, container, savedInstanceState));
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnDestroyView()
        {
            OnDestroyViewCalled.Raise(this);
            base.OnDestroyView();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }
    }
}