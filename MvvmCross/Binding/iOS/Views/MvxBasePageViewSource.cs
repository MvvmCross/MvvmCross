namespace MvvmCross.Binding.iOS.Views
{
    using Platform;
    using System;
    using MvvmCross.Platform.Exceptions;
    using UIKit;

    public abstract class MvxBasePageViewSource : UIPageViewControllerDataSource
    {

        private readonly UIPageViewController _pageView;
        public UIPageViewController PageView => this._pageView;

        protected MvxBasePageViewSource(UIPageViewController pageView)
        {
            this._pageView = pageView;
        }

        public virtual void ReloadData()
        {
            try
            {
                var controller = GetInitialViewController();
                var list = new UIViewController[] { controller };
                // Bug found here: http://stackoverflow.com/questions/15325891/refresh-uipageviewcontroller-reorder-pages-and-add-new-pages
                // has been partially fixed in iOS10 but it is not perfect yet. Let the developer decide what to do.
                SetViewControllers(list);
            }
            catch (Exception exception)
            {
                Mvx.Warning("Exception masked during PageView SetViewControllers {0}", exception.ToLongString());
            }
        }

        protected virtual void SetViewControllers(UIViewController[] viewControllers)
        {
            PageView.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
        }

        protected abstract UIViewController GetViewControllerAtIndex(int index);

        protected virtual UIViewController GetInitialViewController()
        {
            return GetViewControllerAtIndex(0);
        }
    }
}
