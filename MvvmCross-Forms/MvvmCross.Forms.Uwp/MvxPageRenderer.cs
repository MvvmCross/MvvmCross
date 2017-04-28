
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Forms.Views.WindowsUWP
{
    public class MvxPageRenderer : Xamarin.Forms.Platform.UWP.PageRenderer, IMvxBindingContextOwner
    {

        public IMvxBindingContext BindingContext { get; set; }

        protected override void OnElementChanged(Xamarin.Forms.Platform.UWP.ElementChangedEventArgs<Xamarin.Forms.Page> e)
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
