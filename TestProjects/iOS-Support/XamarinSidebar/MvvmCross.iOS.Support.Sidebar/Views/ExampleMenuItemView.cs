using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Support.XamarinSidebar;
using MvvmCross.iOS.Support.XamarinSidebarSample.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    [Register("ExampleMenuItemView")]
    [MvxSidebarPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, false)]
    public class ExampleMenuItemView : BaseViewController<ExampleMenuItemViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var label = new UILabel();

            var bindingSet = this.CreateBindingSet<ExampleMenuItemView, ExampleMenuItemViewModel>();
            bindingSet.Bind(label).To(vm => vm.ExampleValue);
            bindingSet.Apply();

            Add(label);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(

                label.WithSameHeight(View),
                label.WithSameCenterX(View),
                label.WithSameCenterY(View)

                );
        }
    }
}