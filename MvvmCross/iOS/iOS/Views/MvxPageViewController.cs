using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.iOS.Views;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public class MvxPageViewController : MvxEventSourcePageViewController, IMvxIosView
    {
        private readonly Dictionary<string, UIViewController> _pagedViewControllerCache;

        public MvxPageViewController(
            UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll,
            UIPageViewControllerNavigationOrientation orientation =
                UIPageViewControllerNavigationOrientation.Horizontal,
            UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None) : base(style, orientation,
            spine)
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
            get => DataContext as IMvxViewModel;
            set
            {
                DataContext = value;
                //Verify ViewModel is IMvxPageViewModel
                if (DataContext != null && !(DataContext is IMvxPageViewModel))
                    MvxTrace.Error("Error - MvxPageViewController must be given an instance of IMvxPageViewModel");
            }
        }

        public object DataContext
        {
            get => BindingContext.DataContext;
            set => BindingContext.DataContext = value;
        }

        protected virtual void InitializePaging()
        {
            var pageVM = ViewModel as IMvxPageViewModel;
            if (pageVM == null)
                return;
            var defaultVM = pageVM.GetDefaultViewModel();
            var defaultVC = GetViewControllerForViewModel(defaultVM);
            SetViewControllers(new[] {defaultVC}, UIPageViewControllerNavigationDirection.Forward, true, null);
            GetNextViewController = delegate(UIPageViewController pc, UIViewController rc)
            {
                var rcTV = rc as IMvxIosView;
                if (rcTV == null)
                    return null;
                var currentVM = rcTV.ViewModel as IMvxPagedViewModel;
                if (currentVM == null)
                    return null;
                var nextVM = pageVM.GetNextViewModel(currentVM);
                if (nextVM == null)
                    return null;
                var nextVC = GetViewControllerForViewModel(nextVM);
                return nextVC;
            };
            GetPreviousViewController = delegate(UIPageViewController pc, UIViewController rc)
            {
                var rcTV = rc as IMvxIosView;
                if (rcTV == null)
                    return null;
                var currentVM = rcTV.ViewModel as IMvxPagedViewModel;
                if (currentVM == null)
                    return null;
                var prevVM = pageVM.GetPreviousViewModel(currentVM);
                if (prevVM == null)
                    return null;
                var prevVC = GetViewControllerForViewModel(prevVM);
                return prevVC;
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitializePaging();
        }

        public virtual void NavigateToViewModel(IMvxPagedViewModel targetVM,
            UIPageViewControllerNavigationDirection direction, bool animated = true)
        {
            var targetVC = GetViewControllerForViewModel(targetVM);
            SetViewControllers(new[] {targetVC}, direction, animated, null);
        }

        public virtual UIViewController GetViewControllerForViewModel(IMvxPagedViewModel queryVM)
        {
            UIViewController retVal = null;
            if (_pagedViewControllerCache.ContainsKey(queryVM.PagedViewId))
            {
                retVal = _pagedViewControllerCache[queryVM.PagedViewId];
            }
            else
            {
                retVal = this.CreateViewControllerFor(queryVM) as UIViewController;
                _pagedViewControllerCache[queryVM.PagedViewId] = retVal;
            }
            return retVal;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxPageViewController<TViewModel> : MvxPageViewController, IMvxIosView<TViewModel>
        where TViewModel : class, IMvxPageViewModel
    {
        public MvxPageViewController(
            UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll,
            UIPageViewControllerNavigationOrientation orientation =
                UIPageViewControllerNavigationOrientation.Horizontal,
            UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None) : base(style, orientation,
            spine)
        {
        }

        public MvxPageViewController(IntPtr handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}