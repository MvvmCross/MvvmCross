using System;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public static class MvxFormsExtensions
    {
        public static bool IsViewModelTypeOf(this Page page, Type viewModelType)
        {
            return (page as IMvxPage)?.ViewModel.GetType() == viewModelType;
        }
    }
}
