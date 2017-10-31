using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace MvvmCross.Forms.Uwp
{
    public class MvxPageRenderer : PageRenderer, IMvxBindingContextOwner
    {
        public object DataContext
        {
            get
            {
                return BindingContext.DataContext;
            }
            set
            {
                BindingContext.DataContext = value;
            }
        }

        private IMvxBindingContext _bindingContext;
        public IMvxBindingContext BindingContext
        {
            get
            {
                if (_bindingContext == null)
                    BindingContext = new MvxBindingContext();
                return _bindingContext;
            }
            set
            {
                _bindingContext = value;
            }
        }

        public virtual IMvxViewModel ViewModel
        {
            get
            {
                return DataContext as IMvxViewModel;
            }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        protected virtual void OnViewModelSet()
        {
        }
    }

    public class MvxPageRenderer<TViewModel>
        : MvxPageRenderer where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
