using Android.Content;
using Android.Views;
using CrossUI.Core.Elements.Menu;
using CrossUI.Droid.Menus;

namespace Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods
{
    public static class HackMvxMenuExtensionMethods
    {
        public static bool ProcessMenuItemSelected(this IParentMenu parentMenu, IMenuItem item)
        {
#warning TODO - make this OO - let the _parentMenu respond to commands itself...
            foreach (var child in parentMenu.Children)
            {
                var childCast = child as CaptionAndIconMenu;
                if (childCast.UniqueId == item.ItemId)
                {
                    childCast.Command.Execute(null);
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

                if (childCast != null
                    && !string.IsNullOrEmpty(childCast.Icon))
                {
                    var item = menu.Add(1, childCast.UniqueId, 0, childCast.Caption);
#warning TODO - cannot use Resourcein library code! Should we use reflection here? Or some other mechaniasm?
                    var resourceId = context.Resources.GetIdentifier(childCast.Icon, "id", context.PackageName);
                    if (resourceId > 0)
                    {
                        item.SetIcon(resourceId);
                    }
                }
            }
            return true;
        }
    }
}