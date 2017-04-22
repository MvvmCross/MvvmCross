namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    using Cirrious.FluentLayouts.Touch;
    using Foundation;
    using Binding.BindingContext;
    using Core.ViewModels;
    using UIKit;
    using MvvmCross.iOS.Support.XamarinSidebar.Attributes;
    using MvvmCross.iOS.Support.XamarinSidebar;

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