// MvxAndroidBindingResource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace CrossUI.Droid.Dialog
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Android.Runtime;

    using CrossUI.Core;

    public class LinearDialogStyleableResource
    {
        private static bool _initialized = false;

        public static void Initialise()
        {
            try
            {
                if (_initialized)
                    return;

                var resourceType = FindResourceType();

                var styleable = resourceType.GetNestedType("Styleable");

                LinearDialogScrollViewStylableGroupId =
                    (int[])SafeGetFieldValue(styleable, "LinearDialogScrollView", new int[0]);

                LinearDialogScrollViewDivider = (int)SafeGetFieldValue(styleable, "LinearDialogScrollView_divider", -1);
                LinearDialogScrollViewShowDividers = (int)SafeGetFieldValue(styleable, "LinearDialogScrollView_showDividers", -1);
                LinearDialogScrollDividerPadding = (int)SafeGetFieldValue(styleable, "LinearDialogScrollView_dividerPadding", -1);
                LinearDialogScrollDividerHeight = (int)SafeGetFieldValue(styleable, "LinearDialogScrollView_dividerHeight", -1);
                LinearDialogScrollItemBackgroundDrawable = (int)SafeGetFieldValue(styleable, "LinearDialogScrollView_itemBackgroundDrawable", -1);
            }
            catch (Exception exception)
            {
                DialogTrace.WriteLine("Warning - Error finding resource ids for LinearDialog");
                DialogTrace.WriteLine(exception.Message);
            }
            _initialized = true;
        }

        public static int LinearDialogScrollDividerHeight { get; set; }

        public static int LinearDialogScrollItemBackgroundDrawable { get; set; }

        private static Type FindResourceType()
        {
            Func<Assembly, Type> f = ass => ass
                    .GetCustomAttributes(typeof(ResourceDesignerAttribute), true)
                    .OfType<ResourceDesignerAttribute>()
                    .Where(ca => ca.IsApplication)
                    .Select(ca => ass.GetType(ca.FullName))
                    .FirstOrDefault(ty => ty != null);
            return AppDomain.CurrentDomain.GetAssemblies().Select(f).FirstOrDefault(ty => ty != null);
        }

        public static int[] LinearDialogScrollViewStylableGroupId { get; private set; }

        private static object SafeGetFieldValue(Type styleable, string fieldName)
        {
            return SafeGetFieldValue(styleable, fieldName, 0);
        }

        private static object SafeGetFieldValue(Type styleable, string fieldName, object defaultValue)
        {
            var field = styleable.GetField(fieldName);
            if (field == null)
            {
                return defaultValue;
            }

            return field.GetValue(null);
        }

        public static int LinearDialogScrollViewDivider { get; private set; }

        public static int LinearDialogScrollViewShowDividers { get; private set; }

        public static int LinearDialogScrollDividerPadding { get; private set; }
    }
}