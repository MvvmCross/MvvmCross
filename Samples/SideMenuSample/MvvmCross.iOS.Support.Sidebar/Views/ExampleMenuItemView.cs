namespace MvvmCross.iOS.Support.Sidebar.Views
{
    using Cirrious.FluentLayouts.Touch;
    using Foundation;
    using Binding.BindingContext;
    using Core.ViewModels;
    using SidePanels;
    using UIKit;

    [Register("ExampleMenuItemView")]
    [MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, true)]
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