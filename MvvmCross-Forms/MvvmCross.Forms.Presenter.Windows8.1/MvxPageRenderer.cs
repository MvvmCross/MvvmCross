
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Forms.Presenter.Windows81
{
    public class MvxPageRenderer : Xamarin.Forms.Platform.WinRT.PageRenderer, IMvxBindingContextOwner
    {

        public IMvxBindingContext BindingContext { get; set; }

        protected override void OnElementChanged(Xamarin.Forms.Platform.WinRT.ElementChangedEventArgs<Xamarin.Forms.Page> e)
        {
            base.OnElementChanged(e);

            MvxPresenterHelpers.AdaptForBinding(Element, this);
        }
    }

    public class MvxPageRenderer<TViewModel>
        : MvxPageRenderer where TViewModel : class, IMvxViewModel
    {

        public TViewModel ViewModel
        {
            get { return BindingContext.DataContext as TViewModel; }
            set { BindingContext.DataContext = value; }
        }
    }
}
