// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using Microsoft.Extensions.Logging;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using MvvmCross.Platforms.Android.Binding.ResourceHelpers;

namespace MvvmCross.DroidX.RecyclerView.AttributeHelpers
{
    public static class MvxRecyclerViewAttributeExtensions
    {
        private static bool areBindingResourcesInitialized;
        private static int[] _recyclerViewItemTemplateSelectorGroupId;
        private static int _recyclerViewItemTemplateSelector;

        private static string ReadRecyclerViewItemTemplateSelectorClassName(Context context, IAttributeSet attrs)
        {
            TryInitializeBindingResourcePaths();

            TypedArray typedArray = null;

            string className = string.Empty;
            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, _recyclerViewItemTemplateSelectorGroupId);
                int numberOfStyles = typedArray.IndexCount;

                for (int i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == _recyclerViewItemTemplateSelector)
                        className = typedArray.GetString(attributeId);
                }
            }
            finally
            {
                typedArray?.Recycle();
            }

            if (string.IsNullOrEmpty(className))
                return typeof(MvxDefaultTemplateSelector).FullName;

            return className;
        }

        public static IMvxTemplateSelector BuildItemTemplateSelector(Context context, IAttributeSet attrs, int itemTemplateId)
        {
            var templateSelectorClassName = ReadRecyclerViewItemTemplateSelectorClassName(context, attrs);
            var type = Type.GetType(templateSelectorClassName);

            if (type == null)
            {
                const string message =
                    "Sorry but type with class name: {TemplateSelectorClassName} does not exist." +
                    "Make sure you have provided full Type name: namespace + class name, AssemblyName." +
                    "Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
                MvxAndroidLog.Instance.Log(LogLevel.Error, message, templateSelectorClassName);
                throw new InvalidOperationException(message);
            }

            if (!typeof(IMvxTemplateSelector).IsAssignableFrom(type))
            {
                const string message = "Sorry but type: {Type} does not implement {TemplateSelectorType} interface.";
                MvxAndroidLog.Instance.Log(LogLevel.Error, message, type, nameof(IMvxTemplateSelector));
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                const string message = "Sorry can not instatiate {TemplateSelectorType} as provided type: {Type} is abstract/interface.";
                MvxAndroidLog.Instance.Log(LogLevel.Error, message, nameof(IMvxTemplateSelector), type);
                throw new InvalidOperationException(message);
            }

            var templateSelector = Activator.CreateInstance(type) as IMvxTemplateSelector;

            if (itemTemplateId != 0 && templateSelector != null)
                templateSelector.ItemTemplateId = itemTemplateId;

            return templateSelector;
        }

        private static void TryInitializeBindingResourcePaths()
        {
            if (areBindingResourcesInitialized)
                return;
            areBindingResourcesInitialized = true;

            var resourceTypeFinder = Mvx.IoCProvider.Resolve<IMvxAppResourceTypeFinder>().Find();
            var styleableType = resourceTypeFinder.GetNestedType("Styleable");

            _recyclerViewItemTemplateSelectorGroupId = (int[])styleableType.GetField("MvxRecyclerView").GetValue(null);
            _recyclerViewItemTemplateSelector = (int)styleableType.GetField("MvxRecyclerView_MvxTemplateSelector").GetValue(null);
        }
    }
}
