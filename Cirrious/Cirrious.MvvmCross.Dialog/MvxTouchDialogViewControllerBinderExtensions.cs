using System.Linq;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using MonoTouch.Dialog;

namespace Cirrious.MvvmCross.Touch.Dialog
{
    public static class MvxTouchDialogViewControllerBinderExtensions
    {
        public static Element Bind<TViewModel>(this Element element, MvxTouchDialogViewController<TViewModel> controller, string descriptionText)
            where TViewModel : class, IMvxViewModel
        {
            var binder = MvxServiceProviderExtensions.GetService<IMvxBinder>();
            IEnumerable<IMvxBinding> bindings = binder.Bind(controller.ViewModel, element, descriptionText).Select(x => x as IMvxBinding);
			controller.AddBindings(bindings);
            return element;
        }

        public static Element Bind<TViewModel>(this Element element, MvxTouchDialogViewController<TViewModel> controller, IEnumerable<MvxBindingDescription> descriptions)
            where TViewModel : class, IMvxViewModel
        {
            var binder = MvxServiceProviderExtensions.GetService<IMvxBinder>();
            controller.AddBindings(binder.Bind(controller.ViewModel, element, descriptions).Select(x => x as IMvxBinding));
            return element;
        }        
    }
}