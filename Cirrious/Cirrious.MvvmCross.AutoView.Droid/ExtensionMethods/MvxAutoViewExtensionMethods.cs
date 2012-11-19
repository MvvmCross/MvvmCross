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
using FooBar.Dialog.Droid;
using FooBar.Dialog.Droid.Elements;
using IMenu = CrossUI.Core.Elements.Menu.IMenu;

namespace Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods
{
    public static class MvxAutoViewExtensionMethods
    {
        public static IParentMenu LoadMenu<TViewModel>(this IMvxAndroidAutoView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadCommon<TViewModel, ParentMenuDescription, IMenu, IParentMenu>(MvxAutoViewConstants.Menu);
        }

        public static RootElement LoadDialogRoot<TViewModel>(this IMvxAndroidAutoView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadCommon<TViewModel, ElementDescription, IElement, RootElement>(MvxAutoViewConstants.Dialog);
        }

        public static GeneralListLayout LoadList<TViewModel>(this IMvxAndroidAutoView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            return view.LoadCommon<TViewModel, ListLayoutDescription, IListLayout, GeneralListLayout>(MvxAutoViewConstants.List);
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

        private static TResult LoadCommon<TViewModel, TDescription, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view, string key)
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

        private static TResult LoadDialogFromJsonText<TViewModel, TDescription, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view, string jsonText)
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

        private static TResult LoadDialogFromAutoViewModel<TViewModel, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view, string key)
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

        private static TResult LoadDialogRootFromAssets<TViewModel, TDescription, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view, string key)
            where TViewModel : class, IMvxViewModel
            where TDescription : KeyedDescription
        {
            var jsonText = view.GetJsonText(key);
            return view.LoadDialogFromJsonText<TViewModel, TDescription, TBuildable, TResult>(jsonText);
        }

        private static TResult LoadDialogFromDescription<TViewModel, TBuildable, TResult>(this IMvxAndroidAutoView<TViewModel> view,
                                                                         KeyedDescription description)
            where TViewModel : class, IMvxViewModel
        {
            var registry = view.GetService<IBuilderRegistry>();
            var builder = new MvxDroidUserInterfaceBuilder(view, view.ViewModel, registry);
            var root = (TResult)builder.Build(typeof(TBuildable), description);
            return root;
        }

#warning Cull the dead code
/*
        private static void HackFlattenJObjectsToStringDictionary(ListLayoutDescription description)
        {
            if (description == null)
            {
                return;
            }

            if (description.ItemLayouts != null)
            {
                foreach (var layout in description.ItemLayouts)
                {
                    HackFlattenJObjectsToStringDictionary(layout.Value);
                }
            }
            HackFlattenJObjectsToStringDictionary((KeyedDescription)description);
        }

        private static void HackFlattenJObjectsToStringDictionary(KeyedDescription description)
        {
            foreach (var propertyInfo in description.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(description, null);
                var keyedDescription = value as KeyedDescription;
                if (keyedDescription != null)
                {
                    HackFlattenJObjectsToStringDictionary(keyedDescription);
                }
            }

            var flattener = MvxServiceProviderExtensions.GetService<IMvxJsonFlattener>();
            var listToUpdate = new List<KeyValuePair<string, object>>();
            foreach (var prop in description.Properties)
            {
                if (flattener.IsJsonObject(prop.Value))
                {
                    listToUpdate.Add(prop);
                }
            }

            foreach (var keyValuePair in listToUpdate)
            {
                var value = keyValuePair.Value;
                var dict = flattener.FlattenJsonObjectToStringDictionary(value);
                description.Properties[keyValuePair.Key] = dict;
            }
        }
*/

    }
}