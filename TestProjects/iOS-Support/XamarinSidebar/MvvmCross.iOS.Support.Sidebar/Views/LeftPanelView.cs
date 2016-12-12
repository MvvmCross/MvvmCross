namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    using Binding.BindingContext;
    using Cirrious.FluentLayouts.Touch;
    using Core.ViewModels;
    using CoreGraphics;
    using Foundation;
    using SidePanels;
    using UIKit;

    [Register("LeftPanelView")]
    [MvxPanelPresentation(MvxPanelEnum.Left, MvxPanelHintType.ActivePanel, false)]
    public class LeftPanelView : BaseMenuViewController<LeftPanelViewModel>
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

            var centerButton = new UIButton(new CGRect(0, 100, 320, 40));
            centerButton.SetTitle("Master View Menu Item", UIControlState.Normal);
            centerButton.BackgroundColor = UIColor.White;
            centerButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            
            var exampleButton = new UIButton(new CGRect(0, 100, 320, 40));
            exampleButton.SetTitle("Example Menu Item", UIControlState.Normal);
            exampleButton.BackgroundColor = UIColor.White;
            exampleButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            

            var bindingSet = this.CreateBindingSet<LeftPanelView, LeftPanelViewModel>();
            bindingSet.Bind(exampleButton).To(vm => vm.ShowExampleMenuItemCommand);
            bindingSet.Bind(centerButton).To(vm => vm.ShowMasterViewCommand);
            bindingSet.Apply();

            var scrollView = new UIScrollView(View.Frame)
            {
                ShowsHorizontalScrollIndicator = false,
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight
            };
            Add(scrollView);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                scrollView.AtLeftOf(View),
                scrollView.AtTopOf(View),
                scrollView.WithSameWidth(View),
                scrollView.WithSameHeight(View));

            scrollView.AddSubviews(centerButton, exampleButton);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            scrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            var constraints = scrollView.VerticalStackPanelConstraints(new Margins(20, 10, 20, 10, 5, 5), scrollView.Subviews);
            scrollView.AddConstraints(constraints);
        }
    }
}