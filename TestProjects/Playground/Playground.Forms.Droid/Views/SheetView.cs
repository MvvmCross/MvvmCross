using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.Design;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Forms.Droid.Views
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(SheetView))]
    public class SheetView : MvxBottomSheetDialogFragment<SheetViewModel>
    {
        public SheetView()
        {
        }

        protected SheetView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SheetView, null);

            return view;
        }
    }
}
