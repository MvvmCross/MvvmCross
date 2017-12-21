using System;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public static class MvxFormsExtensions
    {
        public static bool PageMatchesViewModel(this Page page, Type viewModelType)
        {
            return (page as IMvxPage)?.ViewModel.GetType() == viewModelType;
        }
    }
}
