// HackMvxMenuExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.ExtensionMethods
{
    using Android.Content;
    using Android.Views;

    using CrossUI.Core.Elements.Menu;
    using CrossUI.Droid.Menus;

    using MvvmCross.Platform.Exceptions;

    public static class HackMvxMenuExtensionMethods
    {
        public static bool ProcessMenuItemSelected(this IParentMenu parentMenu, IMenuItem item)
        {
#warning TODO - make this OO - let the _parentMenu respond to commands itself...
            foreach (var child in parentMenu.Children)
            {
                if (child == null)
                    throw new MvxException("Child is not a CaptionAndIconMenu - is null");

                var childCast = child as CaptionAndIconMenu;
                if (childCast == null)
                    throw new MvxException("Child is not a CaptionAndIconMenu - is a {0}", child.GetType().Name);
                if (childCast != null &&
                    childCast.UniqueId == item.ItemId)
                {
                    childCast.Command?.Execute(null);
                    return true;
                }
            }

            return false;
        }

        public static bool CreateOptionsMenu(this Context context, IParentMenu parentMenu, Android.Views.IMenu menu)
        {
            if (parentMenu == null)
            {
                return false;
            }

#warning TODO - make this OO - let the _parentMenu render itself...
            foreach (var child in parentMenu.Children)
            {
                var childCast = child as CaptionAndIconMenu;

                if (childCast != null)
                {
                    var item = menu.Add(1, childCast.UniqueId, 0, childCast.Caption);
                    if (!string.IsNullOrEmpty(childCast.Icon))
                    {
#warning TODO - cannot use Resourcein library code! Should we use reflection here? Or some other mechaniasm?
                        var resourceId = context.Resources.GetIdentifier(childCast.Icon, "drawable", context.PackageName);
                        if (resourceId > 0)
                        {
                            item.SetIcon(resourceId);
                        }
                    }
                }
            }
            return true;
        }
    }
}