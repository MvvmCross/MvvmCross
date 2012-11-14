using System.Collections.Generic;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.Droid.Views.Lists;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Plugins.Json;
using FooBar.Dialog.Droid;
using Foobar.Dialog.Core.Builder;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Elements;
using Foobar.Dialog.Core.Lists;
using Foobar.Dialog.Core.Menus;
using IMenu = Foobar.Dialog.Core.Menus.IMenu;

namespace Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods
{
    public static class MvxAutoViewExtensionMethods
    {
        public static IParentMenu LoadMenu<TViewModel>(this IMvxTouchAutoView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadCommon<TViewModel, ParentMenuDescription, IMenu, IParentMenu>(MvxAutoViewConstants.Menu);
        }

        public static RootElement LoadDialogRoot<TViewModel>(this IMvxTouchAutoView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadCommon<TViewModel, ElementDescription, IElement, RootElement>(MvxAutoViewConstants.Dialog);
        }

        public static GeneralListLayout LoadList<TViewModel>(this IMvxTouchAutoView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadCommon<TViewModel, ListLayoutDescription, IListLayout, GeneralListLayout>(MvxAutoViewConstants.List);
        }

        private static string GetJsonText<TViewModel>(this IMvxTouchAutoView<TViewModel> view, string key)
            where TViewModel : class, IMvxViewModel
        {
            if (view.ViewModel == null)
            {
                throw new MvxException("You cannot GetJsonText before the ViewModel is set on a IMvxAutoView");
            }

            var defaultViewTextLoader = view.GetService<IMvxAutoViewTextLoader>();
            var json = defaultViewTextLoader.GetDefinition(view.ViewModel.GetType(), key);
            return json;
        }

        private static TResult LoadCommon<TViewModel, TDescription, TBuildable, TResult>(this IMvxTouchAutoView<TViewModel> view, string key)
            where TViewModel : class, IMvxViewModel
            where TDescription : KeyedDescription
            where TResult : class
        {
            var root = view.LoadDialogRootFromAssets<TViewModel, TDescription, TBuildable, TResult>(key);
            if (root != null)
                return root;

            root = LoadDialogFromAutoViewModel<TViewModel, TBuildable, TResult>(view, key);
            if (root != null)
                return root;

            return null;
        }

        private static TResult LoadDialogFromJsonText<TViewModel, TDescription, TBuildable, TResult>(this IMvxTouchAutoView<TViewModel> view, string jsonText)
            where TViewModel : class, IMvxViewModel
            where TDescription : KeyedDescription
        {
            if (string.IsNullOrEmpty(jsonText))
                return default(TResult);

            var json = MvxServiceProviderExtensions.GetService<IMvxJsonConverter>();
            var description = json.DeserializeObject<TDescription>(jsonText);
#warning Hack here - how to flatten these JObjects :/ Maybe need to do it inside the Json converter?
            //HackFlattenJObjectsToStringDictionary(description as ListLayoutDescription);
            var root = view.LoadDialogFromDescription<TViewModel, TBuildable, TResult>(description);
            return root;
        }

        private static TResult LoadDialogFromAutoViewModel<TViewModel, TBuildable, TResult>(this IMvxTouchAutoView<TViewModel> view, string key)
            where TViewModel : class, IMvxViewModel
            where TResult : class
        {
            var autoViewModel = view.ViewModel as IMvxAutoViewModel;
            if (autoViewModel == null)
            {
                return null;
            }

            var description = autoViewModel.GetAutoView(key);
            if (description == null)
            {
                return null;
            }

            return view.LoadDialogFromDescription<TViewModel, TBuildable, TResult>(description);
        }

        private static TResult LoadDialogRootFromAssets<TViewModel, TDescription, TBuildable, TResult>(this IMvxTouchAutoView<TViewModel> view, string key)
            where TViewModel : class, IMvxViewModel
            where TDescription : KeyedDescription
        {
            var jsonText = view.GetJsonText(key);
            return view.LoadDialogFromJsonText<TViewModel, TDescription, TBuildable, TResult>(jsonText);
        }

        private static TResult LoadDialogFromDescription<TViewModel, TBuildable, TResult>(this IMvxTouchAutoView<TViewModel> view,
                                                                         KeyedDescription description)
            where TViewModel : class, IMvxViewModel
        {
            var registry = view.GetService<IBuilderRegistry>();
            var builder = new MvxTouchUserInterfaceBuilder(view, view.ViewModel, registry);
            var root = (TResult)builder.Build(typeof(TBuildable), description);
            return root;
        }


    }
}