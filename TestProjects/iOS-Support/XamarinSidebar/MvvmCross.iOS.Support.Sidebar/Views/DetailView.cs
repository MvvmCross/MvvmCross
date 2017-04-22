namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    using Binding.BindingContext;
    using Cirrious.FluentLayouts.Touch;
    using Core.ViewModels;
    using Foundation;
    using MvvmCross.iOS.Support.XamarinSidebar;
    using MvvmCross.iOS.Support.XamarinSidebar.Attributes;
    using UIKit;

    [Register("DetailView")]
    [MvxSidebarPresentation(MvxPanelEnum.Center, MvxPanelHintType.PushPanel, true, MvxSplitViewBehaviour.Detail)]
    public class DetailView : BaseViewController<DetailViewModel>
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

            View.BackgroundColor = UIColor.Gray;

            var label = new UILabel();

            var bindingSet = this.CreateBindingSet<DetailView, DetailViewModel>();
            bindingSet.Bind(label).To(vm => vm.ExampleValue);

            bindingSet.Apply();

            Add(label);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(

                label.WithSameCenterX(View),
                label.WithSameCenterY(View)

                );
        }
    }
}