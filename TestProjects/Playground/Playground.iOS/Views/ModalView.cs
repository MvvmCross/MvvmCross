using System;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxModalPresentation(ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen, ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve)]
    public partial class ModalView : MvxViewController<ModalViewModel>
    {
        public ModalView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TransitioningDelegate = new TransitioningDelegate();

            View.BackgroundColor = UIColor.Orange;

            var set = this.CreateBindingSet<ModalView, ModalViewModel>();

            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Bind(btnNestedModal).To(vm => vm.ShowNestedModalCommand);

            set.Apply();
        }
    }

    public class TransitioningDelegate : UIViewControllerTransitioningDelegate
    {
        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
        {
            return new CustomTransitionAnimator();
        }
    }

    public class CustomTransitionAnimator : UIViewControllerAnimatedTransitioning
    {
        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return 1.0f;
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var inView = transitionContext.ContainerView;
            var toVC = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            var toView = toVC.View;

            inView.AddSubview(toView);

            var frame = toView.Frame;
            toView.Frame = CGRect.Empty;

            UIView.Animate(TransitionDuration(transitionContext), () =>
            {
                toView.Frame = new CGRect(10, 10, frame.Width - 20, frame.Height - 20);
            }, () =>
            {
                transitionContext.CompleteTransition(true);
            });
        }
    }
}