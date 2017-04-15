namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    using Binding.BindingContext;
    using Cirrious.FluentLayouts.Touch;
    using Core.ViewModels;
    using Foundation;
    using MvvmCross.iOS.Support.XamarinSidebar;
    using MvvmCross.iOS.Support.XamarinSidebar.Attributes;
    using MvvmCross.iOS.Support.XamarinSidebar.Views;
    using MvvmCross.Platform;
    using UIKit;

    [Register("MasterView")]
    [MvxSidebarPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, true, MvxSplitViewBehaviour.Master)]
    public class MasterView : BaseViewController<MasterViewModel>
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

            View.BackgroundColor = UIColor.LightGray;

            var label = new UILabel();

            var detailButton = new UIButton();
            detailButton.SetTitle("Show Detail", UIControlState.Normal);

            var toggleMenuButton = new UIButton();
            toggleMenuButton.SetTitle("Open menu", UIControlState.Normal);
            toggleMenuButton.TouchUpInside += (s, e) =>
            {
                var sideMenu = Mvx.Resolve<IMvxSidebarViewController>();
                sideMenu?.Open(MvxPanelEnum.Left);
            };

            var bindingSet = this.CreateBindingSet<MasterView, MasterViewModel>();
            bindingSet.Bind(label).To(vm => vm.ExampleValue);
            bindingSet.Bind(detailButton).To(vm => vm.ShowDetailCommand);
            bindingSet.Apply();

            Add(label);
            Add(detailButton);
            Add(toggleMenuButton);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(

                label.WithSameCenterX(View),
                label.WithSameCenterY(View),

                detailButton.Below(label, 10),
                detailButton.WithSameCenterX(View),

                toggleMenuButton.Below(detailButton, 10),
                toggleMenuButton.WithSameCenterX(View)

                );
        }
    }
}