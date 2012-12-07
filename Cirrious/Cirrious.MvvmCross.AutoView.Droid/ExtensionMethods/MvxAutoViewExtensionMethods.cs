using System.Collections.Generic;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.Droid.Views.Lists;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Plugins.Json;
using CrossUI.Core.Builder;
using CrossUI.Core.Descriptions;
using CrossUI.Core.Descriptions.Dialog;
using CrossUI.Core.Descriptions.Lists;
using CrossUI.Core.Descriptions.Menu;
using CrossUI.Core.Elements.Dialog;
using CrossUI.Core.Elements.Lists;
using CrossUI.Core.Elements.Menu;
using CrossUI.Droid.Dialog.Elements;
using IMenu = CrossUI.Core.Elements.Menu.IMenu;

namespace Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods
{
    public static class MvxAutoViewExtensionMethods
    {
        public static IParentMenu LoadMenu<TViewModel>(this IMvxAndroidAutoView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadUserInterfaceCommon<TViewModel, ParentMenuDescription, IMenu, IParentMenu>(MvxAutoViewConstants.Menu);
        }

        public static RootElement LoadDialogRoot<TViewModel>(this IMvxAndroidAutoView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadUserInterfaceCommon<TViewModel, ElementDescription, Element, RootElement>(MvxAutoViewConstants.Dialog);
        }

        public static RootElement LoadDialogRoot<TViewModel>(this IMvxAndroidAutoView<TViewModel> view, ElementDescription rootDescription)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadUserInterfaceFromDescription<TViewModel, Element, RootElement>(rootDescription);
        }

        public static GeneralListLayout LoadList<TViewModel>(this IMvxAndroidAutoView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadUserInterfaceCommon<TViewModel, ListLayoutDescription, IListLayout, GeneralListLayout>(MvxAutoViewConstants.List);
        }

        private static string GetJsonText<TViewModel>(this IMvxAndroidAutoView<TViewModel> view, string key)
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

        private static TResult LoadUserInterfaceCommon<TViewModel, TDescription, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view, string key)
            where TViewModel : class, IMvxViewModel
            where TDescription : KeyedDescription
            where TResult : class
        {
            var root = view.LoadUserInterfaceRootFromAssets<TViewModel, TDescription, TBuildable, TResult>(key);
            if (root != null)
                return root;

            root = LoadUserInterfaceFromAutoViewModel<TViewModel, TBuildable, TResult>(view, key);
            if (root != null)
                return root;

            return null;
        }

        private static TResult LoadUserInterfaceFromJsonText<TViewModel, TDescription, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view, string jsonText)
            where TViewModel : class, IMvxViewModel
            where TDescription : KeyedDescription
        {
            if (string.IsNullOrEmpty(jsonText))
                return default(TResult);

            var json = MvxServiceProviderExtensions.GetService<IMvxJsonConverter>();
            var description = json.DeserializeObject<TDescription>(jsonText);
#warning Hack here - how to flatten these JObjects :/ Maybe need to do it inside the Json converter?
            //HackFlattenJObjectsToStringDictionary(description as ListLayoutDescription);
            var root = view.LoadUserInterfaceFromDescription<TViewModel, TBuildable, TResult>(description);
            return root;
        }

        public static TResult LoadUserInterfaceFromAutoViewModel<TViewModel, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view, string key)
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

            return view.LoadUserInterfaceFromDescription<TViewModel, TBuildable, TResult>(description);
        }

        private static TResult LoadUserInterfaceRootFromAssets<TViewModel, TDescription, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view, string key)
            where TViewModel : class, IMvxViewModel
            where TDescription : KeyedDescription
        {
            var jsonText = view.GetJsonText(key);
            return view.LoadUserInterfaceFromJsonText<TViewModel, TDescription, TBuildable, TResult>(jsonText);
        }

        private static TResult LoadUserInterfaceFromDescription<TViewModel, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view,
                                                                         KeyedDescription description)
            where TViewModel : class, IMvxViewModel
        {
            var registry = view.GetService<IBuilderRegistry>();
            var builder = new MvxDroidUserInterfaceBuilder(view, view.ViewModel, registry);
            var root = (TResult)builder.Build(typeof(TBuildable), description);
            return root;
        }
    }
}