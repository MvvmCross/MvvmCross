using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace MvvmCross.Forms.Uwp
{
    public class MvxPageRenderer : PageRenderer, IMvxBindingContextOwner
    {
        public IMvxBindingContext BindingContext { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
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
