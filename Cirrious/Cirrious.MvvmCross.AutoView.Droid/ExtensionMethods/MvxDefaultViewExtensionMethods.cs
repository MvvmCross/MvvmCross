using Android.Content;
using Android.Views;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Plugins.Json;
using FooBar.Dialog.Droid;
using FooBar.Dialog.Droid.Menus;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Elements;
using Foobar.Dialog.Core.Menus;
using IMenu = Foobar.Dialog.Core.Menus.IMenu;

namespace Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods
{
    public static class MvxDefaultViewExtensionMethods
    {
        public static string GetJsonText<TViewModel>(this IMvxDefaultAndroidView<TViewModel> view, string key)
            where TViewModel : class, IMvxViewModel
        {
            if (view.ViewModel == null)
            {
                throw new MvxException("You cannot GetJsonText before the ViewModel is set on a IMvxDefaultView");
            }
            var typeName = view.ViewModel.GetType().Name;
            var defaultViewTextLoader = view.GetService<IMvxDefaultViewTextLoader>();
            var json = defaultViewTextLoader.GetDefinition(typeName, key);
            return json;
        }

        public static IParentMenu LoadMenu<TViewModel>(this IMvxDefaultAndroidView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            var jsonText = view.GetJsonText(MvxDefaultViewConstants.Menu);
            if (string.IsNullOrEmpty(jsonText))
            {
                return null;
            }

#warning Refactor common code here..
            var jsonService = MvxServiceProviderExtensions.GetService<IMvxJsonConverter>(view);
            var description = jsonService.DeserializeObject<ParentMenuDescription>(jsonText);
            var builder = new MvxNewUserInterfaceBuilder(view, view.ViewModel);
            var root = builder.Build(typeof(IMenu), description) as IParentMenu;
            return root;
        }

        public static RootElement LoadDialogRoot<TViewModel>(this IMvxDefaultAndroidView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            var jsonText = view.GetJsonText(MvxDefaultViewConstants.Dialog);
            if (jsonText == null)
                return null;
            var json = MvxServiceProviderExtensions.GetService<IMvxJsonConverter>();
            var description = json.DeserializeObject<ElementDescription>(jsonText);
            var builder = new MvxNewUserInterfaceBuilder(view, view.ViewModel);
            var root = builder.Build(typeof(IElement), description) as RootElement;
            return root;
        }

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
        public static bool CreateOptionsMenu(this Context context,  IParentMenu parentMenu, Android.Views.IMenu menu)
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