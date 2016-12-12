using MvvmCross.iOS.Support.SidePanels;

namespace MvvmCross.iOS.Support.JASidePanelsSample.iOS.Views
{
    using Binding.BindingContext;
    using Cirrious.FluentLayouts.Touch;
    using Foundation;
    using MvvmCross.iOS.Support.JASidePanelsSample.Core.ViewModels;
    using UIKit;

    [Register("LeftPanelView")]
    [MvxPanelPresentation(MvxPanelEnum.Left, MvxPanelHintType.ActivePanel, false)]
    public class LeftPanelView : BaseViewController<LeftPanelViewModel>
    {
        /// <summary>
        /// Called after the controller’s <see cref="P:UIKit.UIViewController.View"/> is loaded into memory.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called after <c>this</c> <see cref="T:UIKit.UIViewController"/>'s <see cref="P:UIKit.UIViewController.View"/> and its entire view hierarchy have been loaded into memory. This method is called whether the <see cref="T:UIKit.UIView"/> was loaded from a .xib file or programmatically.
        /// </para>
        /// </remarks>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var label = new UILabel();

            var bindingSet = this.CreateBindingSet<LeftPanelView, LeftPanelViewModel>();
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