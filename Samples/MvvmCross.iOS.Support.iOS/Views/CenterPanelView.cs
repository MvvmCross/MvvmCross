namespace MvvmCross.iOS.Support.iOS.Views
{
    using Binding.BindingContext;
    using Cirrious.FluentLayouts.Touch;
    using Core.ViewModels;
    using Foundation;
    using Presenters.SidePanels;
    using UIKit;

    [Register("CenterPanelView")]
    [MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ActivePanel, true)]
    public class CenterPanelView : BaseViewController<CenterPanelViewModel>
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

            var rightPanelInstructions = new UILabel();
            rightPanelInstructions.Lines = 0;
            rightPanelInstructions.LineBreakMode = UILineBreakMode.WordWrap;
            rightPanelInstructions.TextAlignment = UITextAlignment.Center;

            var masterButton = new UIButton();
            masterButton.SetTitle("Show Master", UIControlState.Normal);

            var bindingSet = this.CreateBindingSet<CenterPanelView, CenterPanelViewModel>();
            bindingSet.Bind(label).To(vm => vm.ExampleValue);
            bindingSet.Bind(rightPanelInstructions).To(vm => vm.RightPanelInstructions);
            bindingSet.Bind(masterButton).To(vm => vm.ShowMasterCommand);

            Add(label);
            Add(rightPanelInstructions);
            Add(masterButton);
            

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(

                label.WithSameCenterX(View),
                label.WithSameCenterY(View),

                rightPanelInstructions.Below(label, 40),
                rightPanelInstructions.AtLeftOf(View, 20),
                rightPanelInstructions.Width().EqualTo(View.Frame.Width).Minus(40),
                rightPanelInstructions.WithSameCenterX(View),

                masterButton.Below(rightPanelInstructions, 10),
                masterButton.WithSameCenterX(View)

                );

            bindingSet.Apply();
        }
    }
}
