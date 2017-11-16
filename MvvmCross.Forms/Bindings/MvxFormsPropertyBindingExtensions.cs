using System;
using MvvmCross.Forms.Views;

namespace MvvmCross.Forms.Bindings
{
    public static class MvxFormsPropertyBindingExtensions
    {
        public static string BindPartialText(this MvxListView mvxListView)
        => MvxFormsPropertyBinding.MvxListView_ItemClick;
    }
}
