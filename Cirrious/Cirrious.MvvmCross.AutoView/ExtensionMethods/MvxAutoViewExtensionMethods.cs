// MvxAutoViewExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.AutoView.Interfaces;
using CrossUI.Core.Descriptions;
using CrossUI.Core.Descriptions.Dialog;
using CrossUI.Core.Descriptions.Lists;
using CrossUI.Core.Descriptions.Menu;
using CrossUI.Core.Elements.Lists;
using CrossUI.Core.Elements.Menu;

namespace Cirrious.MvvmCross.AutoView.ExtensionMethods
{
    public static class MvxAutoViewExtensionMethods
    {
        public static IParentMenu LoadMenu(this IMvxAutoView view)
        {
            return
                view.LoadUserInterfaceCommon<ParentMenuDescription, IMenu, IParentMenu>(
                    MvxAutoViewConstants.Menu);
        }

        public static TRoot LoadDialogRoot<TElement, TRoot>(this IMvxAutoView view)
            where TRoot : class
        {
            return
                view.LoadUserInterfaceCommon<ElementDescription, TElement, TRoot>(
                    MvxAutoViewConstants.Dialog);
        }

        public static TRoot LoadDialogRoot<TElement, TRoot>(this IMvxAutoView view,
                                                            ElementDescription rootDescription)
        {
            return view.LoadUserInterfaceFromDescription<TElement, TRoot>(rootDescription);
        }

        public static TList LoadList<TList>(this IMvxAutoView view)
            where TList : class
        {
            return
                view.LoadUserInterfaceCommon<ListLayoutDescription, IListLayout, TList>(
                    MvxAutoViewConstants.List);
        }

        private static string GetJsonText(this IMvxAutoView view, string key)
        {
            if (view.ViewModel == null)
            {
                throw new MvxException("You cannot GetJsonText before the ViewModel is set on a IMvxAutoView");
            }

            var defaultViewTextLoader = Mvx.Resolve<IMvxAutoViewTextLoader>();
            var json = defaultViewTextLoader.GetDefinition(view.ViewModel.GetType(), key);
            return json;
        }

        private static TResult LoadUserInterfaceCommon<TDescription, TBuildable, TResult>(
            this IMvxAutoView view, string key)
            where TDescription : KeyedDescription
            where TResult : class
        {
            var root = view.LoadUserInterfaceRootFromAssets<TDescription, TBuildable, TResult>(key);
            if (root != null)
                return root;

            root = LoadUserInterfaceFromAutoViewModel<TBuildable, TResult>(view, key);
            return root;
        }

        private static TResult LoadUserInterfaceFromJsonText<TDescription, TBuildable, TResult>(
            this IMvxAutoView view, string jsonText)
            where TDescription : KeyedDescription
        {
            if (string.IsNullOrEmpty(jsonText))
                return default(TResult);

            var json = Mvx.Resolve<IMvxJsonConverter>();
            var description = json.DeserializeObject<TDescription>(jsonText);
#warning Hack here - how to flatten these JObjects :/ Maybe need to do it inside the Json converter?
            //HackFlattenJObjectsToStringDictionary(description as ListLayoutDescription);
            var root = view.LoadUserInterfaceFromDescription<TBuildable, TResult>(description);
            return root;
        }

        public static TResult LoadUserInterfaceFromAutoViewModel<TBuildable, TResult>(
            this IMvxAutoView view, string key)
            where TResult : class
        {
            var autoViewModel = view.ViewModel as IMvxAutoViewModel;

            var description = autoViewModel?.GetAutoView(key);
            if (description == null)
            {
                return null;
            }

            return view.LoadUserInterfaceFromDescription<TBuildable, TResult>(description);
        }

        private static TResult LoadUserInterfaceRootFromAssets<TDescription, TBuildable, TResult>(
            this IMvxAutoView view, string key)
            where TDescription : KeyedDescription
        {
            var jsonText = view.GetJsonText(key);
            return view.LoadUserInterfaceFromJsonText<TDescription, TBuildable, TResult>(jsonText);
        }

        private static TResult LoadUserInterfaceFromDescription<TBuildable, TResult>(
            this IMvxAutoView view,
            KeyedDescription description)
        {
            var factory = Mvx.Resolve<IMvxUserInterfaceFactory>();
            return factory.Build<TBuildable, TResult>(view, description);
        }
    }
}