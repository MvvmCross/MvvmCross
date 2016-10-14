namespace MvvmCross.iOS.Support.iOS.Views
{
    using SidePanels;
    using Binding.BindingContext;
    using Cirrious.FluentLayouts.Touch;
    using Core.ViewModels;
    using Foundation;
    using UIKit;

    [Register("MasterView")]
    [MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ActivePanel, true, MvxSplitViewBehaviour.Master)]
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

            var bindingSet = this.CreateBindingSet<MasterView, MasterViewModel>();
            bindingSet.Bind(label).To(vm => vm.ExampleValue);
            bindingSet.Bind(detailButton).To(vm => vm.ShowDetailCommand);
            bindingSet.Apply();

            Add(label);
            Add(detailButton);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(

                label.WithSameCenterX(View),
                label.WithSameCenterY(View),

                detailButton.Below(label, 10),
                detailButton.WithSameCenterX(View)

                );
        }
    }
}