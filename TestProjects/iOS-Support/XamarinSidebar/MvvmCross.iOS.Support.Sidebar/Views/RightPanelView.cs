using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Support.XamarinSidebar;
using MvvmCross.iOS.Support.XamarinSidebarSample.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    [Register("RightPanelView")]
    [MvxSidebarPresentation(MvxPanelEnum.Right, MvxPanelHintType.PushPanel, false)]
    public class RightPanelView : BaseMenuViewController<RightPanelViewModel>
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

            var bindingSet = this.CreateBindingSet<RightPanelView, RightPanelViewModel>();
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

        public override UIImage MenuButtonImage => UIImage.FromBundle("twolines");
    }
}