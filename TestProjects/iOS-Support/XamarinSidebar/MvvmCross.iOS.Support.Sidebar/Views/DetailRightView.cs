using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Support.XamarinSidebar;
using MvvmCross.iOS.Support.XamarinSidebarSample.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    [Register("DetailRightView")]
    [MvxSidebarPresentation(MvxPanelEnum.CenterWithRight, MvxPanelHintType.PushPanel, true, MvxSplitViewBehaviour.Detail)]
    public class DetailRightView : BaseViewController<DetailRightViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Gray;

            var label = new UILabel();

            var bindingSet = this.CreateBindingSet<DetailRightView, DetailRightViewModel>();
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