using System;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using MvvmCross.Binding.Droid.ResourceHelpers;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Platform;

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
                return typeof (MvxDefaultTemplateSelector).FullName;

            return className;
        }
            
        public static IMvxTemplateSelector BuildItemTemplateSelector(Context context, IAttributeSet attrs)
        {
            var templateSelectorClassName = ReadRecyclerViewItemTemplateSelectorClassName(context, attrs);
            var type = Type.GetType(templateSelectorClassName);

            if (type == null)
            {
                var message = $"Sorry but type with class name: {templateSelectorClassName} does not exist." +
                             $"Make sure you have provided full Type name: namespace + class name, AssemblyName." +
                              $"Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
                Mvx.Error(message);
                throw new InvalidOperationException(message);
            }
         
            if (!typeof (IMvxTemplateSelector).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(IMvxTemplateSelector)} interface.";
                Mvx.Error(message);
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                string message = $"Sorry can not instatiate {nameof(IMvxTemplateSelector)} as provided type: {type} is abstract/interface.";
                Mvx.Error(message);
                throw new InvalidOperationException(message);
            }

            return Activator.CreateInstance(type) as IMvxTemplateSelector;
        }


        private static bool areBindingResourcesInitialized = false;
        private static void TryInitializeBindingResourcePaths()
        {
            if (areBindingResourcesInitialized)
                return;
            areBindingResourcesInitialized = true;

            var resourceTypeFinder = Mvx.Resolve<IMvxAppResourceTypeFinder>().Find();
            var styleableType = resourceTypeFinder.GetNestedType("Styleable");

            MvxRecyclerViewItemTemplateSelectorGroupId = (int[])styleableType.GetField("MvxRecyclerView").GetValue(null);
            MvxRecyclerViewItemTemplateSelector = (int) styleableType.GetField("MvxRecyclerView_MvxTemplateSelector").GetValue(null);
        }

        private static int[] MvxRecyclerViewItemTemplateSelectorGroupId { get; set; }
        private static int MvxRecyclerViewItemTemplateSelector { get; set; }

    }
}