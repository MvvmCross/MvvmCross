// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.ResourceHelpers;

namespace MvvmCross.Droid.Support.V7.RecyclerView.AttributeHelpers
{
    public static class MvxRecyclerViewAttributeExtensions
    {
        private static string ReadRecyclerViewItemTemplateSelectorClassName(Context context, IAttributeSet attrs)
        {
            TryInitializeBindingResourcePaths();

            TypedArray typedArray = null;

            string className = string.Empty;
            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, MvxRecyclerViewItemTemplateSelectorGroupId);
                int numberOfStyles = typedArray.IndexCount;

                for (int i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == MvxRecyclerViewItemTemplateSelector)
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
                var message = $"Sorry but type with class name: {templateSelectorClassName} does not exist." +
                             $"Make sure you have provided full Type name: namespace + class name, AssemblyName." +
                              $"Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
                MvxAndroidLog.Instance.Error(message);
                throw new InvalidOperationException(message);
            }
         
            if (!typeof(IMvxTemplateSelector).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(IMvxTemplateSelector)} interface.";
                MvxAndroidLog.Instance.Error(message);
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                string message = $"Sorry can not instatiate {nameof(IMvxTemplateSelector)} as provided type: {type} is abstract/interface.";
                MvxAndroidLog.Instance.Error(message);
                throw new InvalidOperationException(message);
            }

            var templateSelector = Activator.CreateInstance(type) as IMvxTemplateSelector;

            if (itemTemplateId != 0 && templateSelector != null)
                    templateSelector.ItemTemplateId = itemTemplateId;

            return templateSelector;
        }

        private static bool areBindingResourcesInitialized = false;
        private static void TryInitializeBindingResourcePaths()
        {
            if (areBindingResourcesInitialized)
                return;
            areBindingResourcesInitialized = true;

            var resourceTypeFinder = Mvx.IoCProvider.Resolve<IMvxAppResourceTypeFinder>().Find();
            var styleableType = resourceTypeFinder.GetNestedType("Styleable");

            MvxRecyclerViewItemTemplateSelectorGroupId = (int[])styleableType.GetField("MvxRecyclerView").GetValue(null);
            MvxRecyclerViewItemTemplateSelector = (int) styleableType.GetField("MvxRecyclerView_MvxTemplateSelector").GetValue(null);
        }

        private static int[] MvxRecyclerViewItemTemplateSelectorGroupId { get; set; }
        private static int MvxRecyclerViewItemTemplateSelector { get; set; }
    }
}
