using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Presenters;

namespace MvvmCross.Forms.Uwp
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
