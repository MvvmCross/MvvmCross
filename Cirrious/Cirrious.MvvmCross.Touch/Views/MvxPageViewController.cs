using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxPageViewController : MvxEventSourcePageViewController, IMvxTouchView
    {
        private Dictionary<string, UIViewController> _pagedViewControllerCache = null;

        public MvxPageViewController(UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None) : base(style, orientation, spine)
        {
            this.AdaptForBinding();
            _pagedViewControllerCache = new Dictionary<string, UIViewController>();
        }

        public MvxPageViewController(IntPtr handle) : base(handle)
        {
            this.AdaptForBinding();
            _pagedViewControllerCache = new Dictionary<string, UIViewController>();
        }

        public MvxViewModelRequest Request { get; set; }
        public IMvxBindingContext BindingContext { get; set; }

        public IMvxViewModel ViewModel
        {
            get { return (DataContext as IMvxViewModel); }
            set
            {
                DataContext = value;
                //Verify ViewModel is IMvxPageViewModel
                if ((DataContext != null) && (!(DataContext is IMvxPageViewModel)))
                    MvxTrace.Error("Error - MvxPageViewController must be given an instance of IMvxPageViewModel");
            }
        }

        public object DataContext
        {
            get { return (BindingContext.DataContext); }
            set { BindingContext.DataContext = value; }
        }

        protected virtual void InitializePaging()
        {
            IMvxPageViewModel pageVM = ViewModel as IMvxPageViewModel;
            if (pageVM == null)
                return;
            IMvxPagedViewModel defaultVM = pageVM.GetDefaultViewModel();
            UIViewController defaultVC = GetViewControllerForViewModel(defaultVM);
            SetViewControllers(new UIViewController[] { defaultVC }, UIPageViewControllerNavigationDirection.Forward, true, null);
            GetNextViewController = delegate (UIPageViewController pc, UIViewController rc)
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
                UIViewController nextVC = GetViewControllerForViewModel(nextVM);
                return (nextVC);
            };
            GetPreviousViewController = delegate (UIPageViewController pc, UIViewController rc)
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
                UIViewController prevVC = GetViewControllerForViewModel(prevVM);
                return (prevVC);
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitializePaging();
        }

        public virtual void NavigateToViewModel(IMvxPagedViewModel targetVM, UIPageViewControllerNavigationDirection direction, bool animated = true)
        {
            UIViewController targetVC = GetViewControllerForViewModel(targetVM);
            SetViewControllers(new UIViewController[] { targetVC }, direction, animated, null);
        }

        public virtual UIViewController GetViewControllerForViewModel(IMvxPagedViewModel queryVM)
        {
            UIViewController retVal = null;
            if (_pagedViewControllerCache.ContainsKey(queryVM.PagedViewId))
                retVal = _pagedViewControllerCache[queryVM.PagedViewId];
            else
            {
                retVal = this.CreateViewControllerFor(queryVM) as UIViewController;
                _pagedViewControllerCache[queryVM.PagedViewId] = retVal;
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