#nullable enable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.Content.Res;
using Android.Util;
using Microsoft.Extensions.Logging;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;

namespace MvvmCross.DroidX.RecyclerView.AttributeHelpers
{
    public static class MvxRecyclerViewAttributeExtensions
    {
        private static bool _areBindingResourcesInitialized;
        private static int[] _recyclerViewItemTemplateSelectorGroupId;
        private static int _recyclerViewItemTemplateSelector;

        private static string ReadRecyclerViewItemTemplateSelectorClassName(Context context, IAttributeSet attrs)
        {
            if (!_areBindingResourcesInitialized)
            {
                if (!TryInitializeBindingResourcePaths(out _recyclerViewItemTemplateSelectorGroupId, out _recyclerViewItemTemplateSelector))
                {
                    _areBindingResourcesInitialized = true;
                    return string.Empty;
                }
                _areBindingResourcesInitialized = true;
            }

            TypedArray? typedArray = null;

            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, _recyclerViewItemTemplateSelectorGroupId);
                var numberOfStyles = typedArray.IndexCount;

                for (var i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);
                    if (attributeId != _recyclerViewItemTemplateSelector) continue;

                    var className = typedArray.GetString(attributeId);
                    if (!string.IsNullOrEmpty(className))
                        return className;
                }
            }
            finally
            {
                typedArray?.Recycle();
            }

            return string.Empty;
        }

        public static IMvxTemplateSelector? BuildItemTemplateSelector(Context context, IAttributeSet attrs, int itemTemplateId)
        {
            var templateSelectorClassName = ReadRecyclerViewItemTemplateSelectorClassName(context, attrs);
            var type = string.IsNullOrEmpty(templateSelectorClassName)
                ? typeof(MvxDefaultTemplateSelector)
                : Type.GetType(templateSelectorClassName);

            if (type == null)
            {
                const string message =
                    "Sorry but type with class name: {TemplateSelectorClassName} does not exist." +
                    "Make sure you have provided full Type name: namespace + class name, AssemblyName." +
                    "Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
                MvxAndroidLog.Instance?.Log(LogLevel.Error, message, templateSelectorClassName);
                throw new InvalidOperationException(message);
            }

            if (!typeof(IMvxTemplateSelector).IsAssignableFrom(type))
            {
                const string message = "Sorry but type: {Type} does not implement {TemplateSelectorType} interface.";
                MvxAndroidLog.Instance?.Log(LogLevel.Error, message, type, nameof(IMvxTemplateSelector));
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                const string message = "Sorry can not instatiate {TemplateSelectorType} as provided type: {Type} is abstract/interface.";
                MvxAndroidLog.Instance?.Log(LogLevel.Error, message, nameof(IMvxTemplateSelector), type);
                throw new InvalidOperationException(message);
            }

            var templateSelector = (IMvxTemplateSelector?)Activator.CreateInstance(type);

            if (itemTemplateId != 0 && templateSelector != null)
                templateSelector.ItemTemplateId = itemTemplateId;

            return templateSelector;
        }

        private static bool TryInitializeBindingResourcePaths(out int[] selectorGroup, out int selector)
        {
            try
            {
#if NET7_0
                if (Mvx.IoCProvider?.TryResolve(
                    out MvvmCross.Platforms.Android.Binding.ResourceHelpers.IMvxAppResourceTypeFinder? resourceTypeFinder) != true)
                {
                    selectorGroup = Array.Empty<int>();
                    selector = 0;
                    return false;
                }

                var resourceType = resourceTypeFinder?.Find();
                if (resourceType == null)
                {
                    MvxAndroidLog.Instance?.LogWarning("Could not find Resource Type - MvxRecyclerView binding won't work correctly");
                    selectorGroup = Array.Empty<int>();
                    selector = 0;
                    return false;
                }

                var styleableType = resourceType.GetNestedType("Styleable");
                if (styleableType == null)
                {
                    MvxAndroidLog.Instance?.LogWarning("Could not find Styleable Type - MvxRecyclerView binding won't work correctly");
                    selectorGroup = Array.Empty<int>();
                    selector = 0;
                    return false;
                }

                MvxAndroidLog.Instance?.LogTrace("Styleable Type found: {Type}", styleableType.FullName);
                selectorGroup = (int[])(styleableType.GetField("MvxRecyclerView")?.GetValue(null) ?? Array.Empty<int>());
                selector = (int)(styleableType.GetField("MvxRecyclerView_MvxTemplateSelector")?.GetValue(null) ?? 0);
                return true;
#elif NET8_0_OR_GREATER
                var styleableType = typeof(global::_Microsoft.Android.Resource.Designer.Resource).GetNestedType("Styleable");
                if (styleableType == null)
                {
                    MvxAndroidLog.Instance?.LogWarning("Could not find Styleable Type - MvxRecyclerView binding won't work correctly");
                    selectorGroup = Array.Empty<int>();
                    selector = 0;
                    return false;
                }
                MvxAndroidLog.Instance?.LogTrace("Styleable Type found: {Type}", styleableType.FullName);

                selectorGroup = (int[])(styleableType.GetProperty("MvxRecyclerView")?.GetValue(null) ?? Array.Empty<int>());
                selector = (int)(styleableType.GetProperty("MvxRecyclerView_MvxTemplateSelector")?.GetValue(null) ?? 0);
                return true;
#endif
            }
            catch (Exception e)
            {
                MvxAndroidLog.Instance?.LogError(e, "Failed to initialize MvxRecyclerView binding resources");
            }

            selectorGroup = Array.Empty<int>();
            selector = 0;
            return false;
        }
    }
}
