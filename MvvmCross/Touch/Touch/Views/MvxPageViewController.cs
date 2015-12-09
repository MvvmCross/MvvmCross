namespace MvvmCross.Touch.Views
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Touch.Views;

    using UIKit;

    public class MvxPageViewController : MvxEventSourcePageViewController, IMvxTouchView
    {
        private Dictionary<string, UIViewController> _pagedViewControllerCache = null;

        public MvxPageViewController(UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None) : base(style, orientation, spine)
        {
            this.AdaptForBinding();
            this._pagedViewControllerCache = new Dictionary<string, UIViewController>();
        }

        public MvxPageViewController(IntPtr handle) : base(handle)
        {
            this.AdaptForBinding();
            this._pagedViewControllerCache = new Dictionary<string, UIViewController>();
        }

        public MvxViewModelRequest Request { get; set; }
        public IMvxBindingContext BindingContext { get; set; }

        public IMvxViewModel ViewModel
        {
            get { return (this.DataContext as IMvxViewModel); }
            set
            {
                this.DataContext = value;
                //Verify ViewModel is IMvxPageViewModel
                if ((this.DataContext != null) && (!(this.DataContext is IMvxPageViewModel)))
                    MvxTrace.Error("Error - MvxPageViewController must be given an instance of IMvxPageViewModel");
            }
        }

        public object DataContext
        {
            get { return (this.BindingContext.DataContext); }
            set { this.BindingContext.DataContext = value; }
        }

        protected virtual void InitializePaging()
        {
            IMvxPageViewModel pageVM = this.ViewModel as IMvxPageViewModel;
            if (pageVM == null)
                return;
            IMvxPagedViewModel defaultVM = pageVM.GetDefaultViewModel();
            UIViewController defaultVC = this.GetViewControllerForViewModel(defaultVM);
            this.SetViewControllers(new UIViewController[] { defaultVC }, UIPageViewControllerNavigationDirection.Forward, true, null);
            this.GetNextViewController = delegate (UIPageViewController pc, UIViewController rc)
            {
                IMvxTouchView rcTV = rc as IMvxTouchView;
                if (rcTV == null)
                    return (null);
                IMvxPagedViewModel currentVM = rcTV.ViewModel as IMvxPagedViewModel;
                if (currentVM == null)
                    return (null);
                IMvxPagedViewModel nextVM = pageVM.GetNextViewModel(currentVM);
                if (nextVM == null)
                    return (null);
                UIViewController nextVC = this.GetViewControllerForViewModel(nextVM);
                return (nextVC);
            };
            this.GetPreviousViewController = delegate (UIPageViewController pc, UIViewController rc)
            {
                IMvxTouchView rcTV = rc as IMvxTouchView;
                if (rcTV == null)
                    return (null);
                IMvxPagedViewModel currentVM = rcTV.ViewModel as IMvxPagedViewModel;
                if (currentVM == null)
                    return (null);
                IMvxPagedViewModel prevVM = pageVM.GetPreviousViewModel(currentVM);
                if (prevVM == null)
                    return (null);
                UIViewController prevVC = this.GetViewControllerForViewModel(prevVM);
                return (prevVC);
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.InitializePaging();
        }

        public virtual void NavigateToViewModel(IMvxPagedViewModel targetVM, UIPageViewControllerNavigationDirection direction, bool animated = true)
        {
            UIViewController targetVC = this.GetViewControllerForViewModel(targetVM);
            this.SetViewControllers(new UIViewController[] { targetVC }, direction, animated, null);
        }

        public virtual UIViewController GetViewControllerForViewModel(IMvxPagedViewModel queryVM)
        {
            UIViewController retVal = null;
            if (this._pagedViewControllerCache.ContainsKey(queryVM.PagedViewId))
                retVal = this._pagedViewControllerCache[queryVM.PagedViewId];
            else
            {
                retVal = this.CreateViewControllerFor(queryVM) as UIViewController;
                this._pagedViewControllerCache[queryVM.PagedViewId] = retVal;
            }
            return (retVal);
        }
    }

    public class MvxPageViewController<TViewModel> : MvxPageViewController, IMvxTouchView<TViewModel> where TViewModel : class, IMvxPageViewModel
    {
        public MvxPageViewController(UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None) : base(style, orientation, spine)
        {
        }

        public MvxPageViewController(IntPtr handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}