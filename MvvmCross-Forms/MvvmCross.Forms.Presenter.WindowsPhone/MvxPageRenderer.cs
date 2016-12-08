
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Forms.Presenter.WindowsPhone
{
    public class MvxPageRenderer : Xamarin.Forms.Platform.WinPhone.PageRenderer, IMvxBindingContextOwner
    {

        public IMvxBindingContext BindingContext { get; set; }

        protected override void OnElementChanged(Xamarin.Forms.Platform.WinPhone.ElementChangedEventArgs<Xamarin.Forms.Page> e)
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
