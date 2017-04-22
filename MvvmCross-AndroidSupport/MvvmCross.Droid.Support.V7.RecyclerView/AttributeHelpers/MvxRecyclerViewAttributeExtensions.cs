using System;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using MvvmCross.Binding.Droid.ResourceHelpers;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Platform;

namespace MvvmCross.Droid.Support.V7.RecyclerView.AttributeHelpers
{
    internal static class MvxRecyclerViewAttributeExtensions
    {
        private static MvxRecyclerViewTemplateSelectorAttributes ReadRecyclerViewItemTemplateSelectorAttributes(Context context, IAttributeSet attrs)
        {
            TryInitializeBindingResourcePaths();

            TypedArray typedArray = null;

            string templateSelectorClassName = string.Empty;
            string groupedDataConverterClassName = string.Empty;
            int headerLayoutId = 0;
            int footerLayoutId = 0;
            int groupSectionLayoutId = 0;

            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, MvxRecyclerViewGroupId);
                int numberOfStyles = typedArray.IndexCount;

                for (int i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == MvxRecyclerViewItemTemplateSelector)
                        templateSelectorClassName = typedArray.GetString(attributeId);
                    if (attributeId == MvxRecyclerViewHeaderLayoutId)
                        headerLayoutId = typedArray.GetResourceId(attributeId, 0);
                    if (attributeId == MvxRecyclerViewFooterLayoutId)
                        footerLayoutId = typedArray.GetResourceId(attributeId, 0);
                    if (attributeId == MvxRecyclerViewGroupSectionLayoutId)
                        groupSectionLayoutId = typedArray.GetResourceId(attributeId, 0);
                    if (attributeId == MvxRecyclerViewGroupedDataConverter)
                        groupedDataConverterClassName = typedArray.GetString(attributeId);
                }
            }
            finally
            {
                typedArray?.Recycle();
            }

            if (string.IsNullOrEmpty(templateSelectorClassName))
                templateSelectorClassName = typeof(MvxDefaultTemplateSelector).FullName;

            return new MvxRecyclerViewTemplateSelectorAttributes()
            {
                TemplateSelectorClassName = templateSelectorClassName,
                FooterLayoutId = footerLayoutId,
                HeaderLayoutId = headerLayoutId,
                GroupSectionLayoutId = groupSectionLayoutId,
                GroupedDataConverterClassName = groupedDataConverterClassName
            };
        }

        public static MvxBaseTemplateSelector BuildItemTemplateSelector(Context context, IAttributeSet attrs)
        {
            var templateSelectorAttributes = ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs);
            var type = Type.GetType(templateSelectorAttributes.TemplateSelectorClassName);

            if (type == null)
            {
                var message = $"Sorry but type with class name: {templateSelectorAttributes} does not exist." +
                             $"Make sure you have provided full Type name: namespace + class name, AssemblyName." +
                              $"Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
                Mvx.Error(message);
                throw new InvalidOperationException(message);
            }

            if (!typeof(MvxBaseTemplateSelector).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(MvxBaseTemplateSelector)} interface.";
                Mvx.Error(message);
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                string message = $"Sorry can not instatiate {nameof(MvxBaseTemplateSelector)} as provided type: {type} is abstract/interface.";
                Mvx.Error(message);
                throw new InvalidOperationException(message);
            }

            var templateSelector = Activator.CreateInstance(type) as MvxBaseTemplateSelector;
            templateSelector.FooterLayoutId = templateSelectorAttributes.FooterLayoutId;
            templateSelector.HeaderLayoutId = templateSelectorAttributes.HeaderLayoutId;
            templateSelector.GroupSectionLayoutId = templateSelectorAttributes.GroupSectionLayoutId;
            return templateSelector;
        }

        public static bool IsGroupingSupported(Context context, IAttributeSet attrs)
        {
            return
                !string.IsNullOrEmpty(ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs).GroupedDataConverterClassName);
        }

        public static IMvxGroupedDataConverter BuildMvxGroupedDataConverter(Context context, IAttributeSet attrs)
        {
            var groupedDataConverterClassName = ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs).GroupedDataConverterClassName;
            var type = Type.GetType(groupedDataConverterClassName);

            if (type == null)
            {
                var message = $"Sorry but type with class name: {groupedDataConverterClassName} does not exist." +
                              $"Make sure you have provided full Type name: namespace + class name, AssemblyName.";
                Mvx.Error(message);
                throw new InvalidOperationException(message);
            }

            if (!typeof(IMvxGroupedDataConverter).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(IMvxGroupedDataConverter)} interface.";
                Mvx.Error(message);
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                string message = $"Sorry can not instatiate {nameof(IMvxGroupedDataConverter)} as provided type: {type} is abstract/interface.";
                Mvx.Error(message);
                throw new InvalidOperationException(message);
            }

            return Activator.CreateInstance(type) as IMvxGroupedDataConverter;
        }

        public static bool IsHidesHeaderIfEmptyEnabled(Context context, IAttributeSet attrs)
        {
            TryInitializeBindingResourcePaths();

            TypedArray typedArray = null;
            bool hidesHeaderIfEmpty = true;

            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, MvxRecyclerViewGroupId);
                int numberOfStyles = typedArray.IndexCount;

                for (int i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == MvxRecyclerViewHidesHeaderIfEmpty)
                    {
                        hidesHeaderIfEmpty = typedArray.GetBoolean(attributeId, true);
                        break;
                    }
                }

                return hidesHeaderIfEmpty;
            }
            finally
            {
                typedArray.Recycle();
            }
        }


        public static bool IsHidesFooterIfEmptyEnabled(Context context, IAttributeSet attrs)
        {
            TryInitializeBindingResourcePaths();

            TypedArray typedArray = null;
            bool hidesFooterIfEmpty = true;

            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, MvxRecyclerViewGroupId);
                int numberOfStyles = typedArray.IndexCount;

                for (int i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == MvxRecyclerViewHidesFooterIfEmpty)
                    {
                        hidesFooterIfEmpty = typedArray.GetBoolean(attributeId, true);
                        break;
                    }
                }

                return hidesFooterIfEmpty;
            }
            finally
            {
                typedArray.Recycle();
            }
        }

        private static bool areBindingResourcesInitialized = false;
        private static void TryInitializeBindingResourcePaths()
        {
            if (areBindingResourcesInitialized)
                return;
            areBindingResourcesInitialized = true;

            var resourceTypeFinder = Mvx.Resolve<IMvxAppResourceTypeFinder>().Find();
            var styleableType = resourceTypeFinder.GetNestedType("Styleable");

            MvxRecyclerViewGroupId = (int[])styleableType.GetField("MvxRecyclerView").GetValue(null);
            MvxRecyclerViewItemTemplateSelector = (int)styleableType.GetField("MvxRecyclerView_MvxTemplateSelector").GetValue(null);
            MvxRecyclerViewHeaderLayoutId = (int)styleableType.GetField("MvxRecyclerView_MvxHeaderLayoutId").GetValue(null);
            MvxRecyclerViewFooterLayoutId = (int)styleableType.GetField("MvxRecyclerView_MvxFooterLayoutId").GetValue(null);
            MvxRecyclerViewGroupedDataConverter =
                (int)styleableType.GetField("MvxRecyclerView_MvxGroupedDataConverter").GetValue(null);
            MvxRecyclerViewGroupSectionLayoutId = (int)styleableType.GetField("MvxRecyclerView_MvxGroupSectionLayoutId").GetValue(null);
            MvxRecyclerViewHidesHeaderIfEmpty =
                (int)styleableType.GetField("MvxRecyclerView_MvxHidesHeaderIfEmpty").GetValue(null);
            MvxRecyclerViewHidesFooterIfEmpty =
                (int)styleableType.GetField("MvxRecyclerView_MvxHidesFooterIfEmpty").GetValue(null);
        }

        private static int[] MvxRecyclerViewGroupId { get; set; }
        private static int MvxRecyclerViewItemTemplateSelector { get; set; }

        private static int MvxRecyclerViewHeaderLayoutId { get; set; }

        private static int MvxRecyclerViewFooterLayoutId { get; set; }

        private static int MvxRecyclerViewGroupSectionLayoutId { get; set; }

        private static int MvxRecyclerViewHidesHeaderIfEmpty { get; set; }

        private static int MvxRecyclerViewHidesFooterIfEmpty { get; set; }

        public static int MvxRecyclerViewGroupedDataConverter { get; set; }
    }
}