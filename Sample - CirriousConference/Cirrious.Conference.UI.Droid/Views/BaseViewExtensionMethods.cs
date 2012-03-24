using System.Globalization;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Converters;
using Cirrious.MvvmCross.Interfaces.Localization;

namespace Cirrious.Conference.UI.Droid.Views
{
    public static class BaseViewExtensionMethods
    {
        private static readonly MvxLanguageBinderConverter _converter = new MvxLanguageBinderConverter();

        public static string GetText<TViewModel>(this IBaseView<TViewModel> view, string which)
            where TViewModel : BaseViewModel
        {
            return GetTextCommon(view.ViewModel.TextSource, which);
        }

        public static string GetSharedText<TViewModel>(this IBaseView<TViewModel> view, string which)
            where TViewModel : BaseViewModel
        {
            return GetTextCommon(view.ViewModel.SharedTextSource, which);
        }

        private static string GetTextCommon(IMvxLanguageBinder source, string which)
        {
            return (string)_converter.Convert(source, typeof (string), which, CultureInfo.CurrentUICulture);
        }
    }
}