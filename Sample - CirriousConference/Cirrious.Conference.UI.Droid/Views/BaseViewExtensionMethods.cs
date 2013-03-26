using System.Globalization;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Localization;

namespace Cirrious.Conference.UI.Droid.Views
{
    public static class BaseViewExtensionMethods
    {
        private static readonly MvxLanguageConverter _converter = new MvxLanguageConverter();

        public static string GetText<TViewModel>(this IBaseView<TViewModel> view, string which)
            where TViewModel : BaseViewModel
        {
            return GetTextCommon(((BaseViewModel)view.ViewModel).TextSource, which);
        }

        public static string GetSharedText<TViewModel>(this IBaseView<TViewModel> view, string which)
            where TViewModel : BaseViewModel
        {
            return GetTextCommon(((BaseViewModel)view.ViewModel).SharedTextSource, which);
        }

        private static string GetTextCommon(IMvxLanguageBinder source, string which)
        {
            return (string)_converter.Convert(source, typeof (string), which, CultureInfo.CurrentUICulture);
        }
    }
}