﻿// MvxAndroidBindingResource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid.ResourceHelpers
{ 
    public class MvxAndroidBindingResource
        : MvxSingleton<IMvxAndroidBindingResource>
        , IMvxAndroidBindingResource
    {
        public static void Initialize()
        {
            if (Instance != null)
                return;

            var androidBindingResource = new MvxAndroidBindingResource();
        }

        private MvxAndroidBindingResource()
        {
            var finder = Mvx.Resolve<IMvxAppResourceTypeFinder>();
            var resourceType = finder.Find();
            try
            {
                var id = resourceType.GetNestedType("Id");
                BindingTagUnique = (int)SafeGetFieldValue(id, "MvxBindingTagUnique");

                var styleable = resourceType.GetNestedType("Styleable");

                ControlStylableGroupId =
                    (int[])SafeGetFieldValue(styleable, "MvxControl", new int[0]);
                TemplateId =
                    (int)SafeGetFieldValue(styleable, "MvxControl_MvxTemplate");

                BindingStylableGroupId =
                    (int[])SafeGetFieldValue(styleable, "MvxBinding", new int[0]);
                BindingBindId =
                    (int)SafeGetFieldValue(styleable, "MvxBinding_MvxBind");
                BindingLangId =
                    (int)SafeGetFieldValue(styleable, "MvxBinding_MvxLang");

                ImageViewStylableGroupId =
                    (int[])SafeGetFieldValue(styleable, "MvxImageView", new int[0]);
                SourceBindId =
                    (int)
                    SafeGetFieldValue(styleable, "MvxImageView_MvxSource");

                ListViewStylableGroupId =
                    (int[])SafeGetFieldValue(styleable, "MvxListView");
                ListItemTemplateId =
                    (int)
                    styleable
                        .GetField("MvxListView_MvxItemTemplate")
                        .GetValue(null);
                DropDownListItemTemplateId =
                    (int)
                    styleable
                        .GetField("MvxListView_MvxDropDownItemTemplate")
                        .GetValue(null);

                ExpandableListViewStylableGroupId =
                    (int[])SafeGetFieldValue(styleable, "MvxExpandableListView", new int[0]);
                GroupItemTemplateId =
                    (int)SafeGetFieldValue(styleable, "MvxExpandableListView_MvxGroupItemTemplate");
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    "Error finding resource ids for MvxBinding - please make sure ResourcesToCopy are linked into the executable");
            }
        }

        private static object SafeGetFieldValue(Type styleable, string fieldName)
        {
            return SafeGetFieldValue(styleable, fieldName, 0);
        }

        private static object SafeGetFieldValue(Type styleable, string fieldName, object defaultValue)
        {
            var field = styleable.GetField(fieldName);
            if (field == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Missing stylable field {0}", fieldName);
                return defaultValue;
            }

            return field.GetValue(null);
        }

        public int BindingTagUnique { get; }

        public int[] BindingStylableGroupId { get; }
        public int BindingBindId { get; }
        public int BindingLangId { get; }

        public int[] ControlStylableGroupId { get; }
        public int TemplateId { get; }

        public int[] ImageViewStylableGroupId { get; }
        public int SourceBindId { get; }

        public int[] ListViewStylableGroupId { get; }
        public int ListItemTemplateId { get; }
        public int DropDownListItemTemplateId { get; }

        public int[] ExpandableListViewStylableGroupId { get; }
        public int GroupItemTemplateId { get; }
    }
}
