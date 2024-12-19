// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

namespace MvvmCross.Platforms.Ios.Views;

public static class ViewExtensions
{
    /// <summary>
    /// Find the first responder in the <paramref name="view"/>'s subview hierarchy
    /// </summary>
    /// <param name="view">
    /// A <see cref="UIView"/>
    /// </param>
    /// <returns>
    /// A <see cref="UIView"/> that is the first responder or null if there is no first responder
    /// </returns>
    public static UIView? FindFirstResponder(this UIView view)
    {
        if (view.IsFirstResponder)
        {
            return view;
        }

        return view.Subviews.Select(subView => subView.FindFirstResponder()).FirstOrDefault(firstResponder => firstResponder != null);
    }

    /// <summary>
    /// Find the first Superview of the specified type (or descendant of)
    /// </summary>
    /// <param name="view">
    /// A <see cref="UIView"/>
    /// </param>
    /// <param name="stopAt">
    /// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
    /// </param>
    /// <param name="type">
    /// A <see cref="Type"/> to look for, this should be a UIView or descendant type
    /// </param>
    /// <returns>
    /// A <see cref="UIView"/> if it is found, otherwise null
    /// </returns>
    public static UIView? FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
    {
        if (type.IsInstanceOfType(view.Superview))
        {
            return view.Superview;
        }

        if (!Equals(view.Superview, stopAt))
        {
            return view.Superview.FindSuperviewOfType(stopAt, type);
        }

        return null;
    }

    public static UIView? FindTopSuperviewOfType(this UIView view, UIView stopAt, Type type)
    {
        var superview = view.FindSuperviewOfType(stopAt, type);
        var topSuperView = superview;
        while (superview != null && !Equals(superview, stopAt))
        {
            superview = superview.FindSuperviewOfType(stopAt, type);
            if (superview != null)
            {
                topSuperView = superview;
            }
        }

        return topSuperView;
    }

    public static void RestoreScrollPosition(this UIScrollView scrollView)
    {
        scrollView.ContentInset = UIEdgeInsets.Zero;
        scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
    }

    public static bool IsLandscape()
    {
        var orientation = UIDevice.CurrentDevice.Orientation;
        return orientation is UIDeviceOrientation.LandscapeLeft or UIDeviceOrientation.LandscapeRight;
    }

    /// <summary>
    /// Finds all descendants of a type within the view
    /// </summary>
    /// <returns>List of views of the type</returns>
    /// <param name="view">View</param>
    /// <typeparam name="T">The type to find.</typeparam>
    public static List<T> DescendantViewsOfType<T>(this UIView view) where T : UIView
    {
        return view.DescendantViews().OfType<T>().ToList();
    }

    private static List<UIView> DescendantViews(this UIView view)
    {
        var descendantViews = new List<UIView>();

        if (view.Subviews.Any())
        {
            descendantViews.AddRange(view.Subviews);

            foreach (var subview in view.Subviews)
            {
                descendantViews.AddRange(subview.DescendantViews());
            }
        }

        return descendantViews;
    }

    public static UIViewController? GetTopModalHostViewController(this UIWindow window)
    {
        var vc = window.RootViewController;
        do
        {
            if (vc?.PresentedViewController != null)
                vc = vc.PresentedViewController;
        } while (vc?.PresentedViewController != null);

        return vc;
    }
}
