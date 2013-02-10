// MvxAutoViewExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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
using CrossUI.Core.Elements.Lists;
using CrossUI.Core.Elements.Menu;
using CrossUI.Droid.Dialog.Elements;

namespace Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods
{
    public static class MvxAutoViewExtensionMethods
    {
        public static IParentMenu LoadMenu(this IMvxAndroidAutoView view)
        {
            return
                view.LoadUserInterfaceCommon<ParentMenuDescription, IMenu, IParentMenu>(
                    MvxAutoViewConstants.Menu);
        }

        public static RootElement LoadDialogRoot(this IMvxAndroidAutoView view)
        {
            return
                view.LoadUserInterfaceCommon<ElementDescription, Element, RootElement>(
                    MvxAutoViewConstants.Dialog);
        }

        public static RootElement LoadDialogRoot(this IMvxAndroidAutoView view,
                                                             ElementDescription rootDescription)
        {
            return view.LoadUserInterfaceFromDescription<Element, RootElement>(rootDescription);
        }

        public static GeneralListLayout LoadList(this IMvxAndroidAutoView view)
        {
            return
                view.LoadUserInterfaceCommon<ListLayoutDescription, IListLayout, GeneralListLayout>(
                    MvxAutoViewConstants.List);
        }

        private static string GetJsonText(this IMvxAndroidAutoView view, string key)
        {
            if (view.ViewModel == null)
            {
                throw new MvxException("You cannot GetJsonText before the ViewModel is set on a IMvxAutoView");
            }

            var defaultViewTextLoader = view.GetService<IMvxAutoViewTextLoader>();
            var json = defaultViewTextLoader.GetDefinition(view.ViewModel.GetType(), key);
            return json;
        }

        private static TResult LoadUserInterfaceCommon<TDescription, TBuildable, TResult>(
            this IMvxAndroidAutoView view, string key)
            where TDescription : KeyedDescription
            where TResult : class
        {
            var root = view.LoadUserInterfaceRootFromAssets<TDescription, TBuildable, TResult>(key);
            if (root != null)
                return root;

            root = LoadUserInterfaceFromAutoViewModel<TBuildable, TResult>(view, key);
            if (root != null)
                return root;

            return null;
        }

        private static TResult LoadUserInterfaceFromJsonText<TDescription, TBuildable, TResult>(
            this IMvxAndroidAutoView view, string jsonText)
            where TDescription : KeyedDescription
        {
            if (string.IsNullOrEmpty(jsonText))
                return default(TResult);

            var json = MvxServiceProviderExtensions.GetService<IMvxJsonConverter>();
            var description = json.DeserializeObject<TDescription>(jsonText);
#warning Hack here - how to flatten these JObjects :/ Maybe need to do it inside the Json converter?
            //HackFlattenJObjectsToStringDictionary(description as ListLayoutDescription);
            var root = view.LoadUserInterfaceFromDescription<TBuildable, TResult>(description);
            return root;
        }

        public static TResult LoadUserInterfaceFromAutoViewModel<TBuildable, TResult>(
            this IMvxAndroidAutoView view, string key)
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

            return view.LoadUserInterfaceFromDescription<TBuildable, TResult>(description);
        }

        private static TResult LoadUserInterfaceRootFromAssets<TDescription, TBuildable, TResult>(
            this IMvxAndroidAutoView view, string key)
            where TDescription : KeyedDescription
        {
            var jsonText = view.GetJsonText(key);
            return view.LoadUserInterfaceFromJsonText<TDescription, TBuildable, TResult>(jsonText);
        }

        private static TResult LoadUserInterfaceFromDescription<TBuildable, TResult>(
            this IMvxAndroidAutoView view,
            KeyedDescription description)
        {
            var registry = view.GetService<IBuilderRegistry>();
            var builder = new MvxDroidUserInterfaceBuilder(view, view.ViewModel, registry);
            var root = (TResult) builder.Build(typeof (TBuildable), description);
            return root;
        }
    }
}