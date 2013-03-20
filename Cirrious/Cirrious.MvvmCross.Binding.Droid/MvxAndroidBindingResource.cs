// MvxAndroidBindingResource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Droid
{
    public class MvxAndroidBindingResource
    {
        public static readonly MvxAndroidBindingResource Instance = new MvxAndroidBindingResource();

        private MvxAndroidBindingResource()
        {
            var setup = Mvx.Resolve<IMvxAndroidGlobals>();
            var resourceTypeName = setup.ExecutableNamespace + ".Resource";
            Type resourceType = setup.ExecutableAssembly.GetType(resourceTypeName);
            if (resourceType == null)
                throw new MvxException("Unable to find resource type - " + resourceTypeName);
            try
            {
                var id = resourceType.GetNestedType("Id");
                BindingTagUnique = (int) SafeGetFieldValue(id, "MvxBindingTagUnique");

                var styleable = resourceType.GetNestedType("Styleable");

                BindingStylableGroupId =
                    (int[]) SafeGetFieldValue(styleable, "MvxBinding", new int[0]);
                BindingBindId =
                    (int) SafeGetFieldValue(styleable, "MvxBinding_MvxBind");
                BindingLangId =
                    (int) SafeGetFieldValue(styleable, "MvxBinding_MvxLang");

                ImageViewStylableGroupId =
                    (int[]) SafeGetFieldValue(styleable, "MvxImageView", new int[0]);
                SourceBindId =
                    (int)
                    SafeGetFieldValue(styleable, "MvxImageView_MvxSource");

                ListViewStylableGroupId =
                    (int[]) SafeGetFieldValue(styleable, "MvxListView");
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

        public int BindingTagUnique { get; private set; }

        public int[] BindingStylableGroupId { get; private set; }
        public int BindingBindId { get; private set; }
        public int BindingLangId { get; private set; }

        public int[] ImageViewStylableGroupId { get; private set; }
        public int SourceBindId { get; private set; }

        public int[] ListViewStylableGroupId { get; private set; }
        public int ListItemTemplateId { get; private set; }
        public int DropDownListItemTemplateId { get; private set; }
    }
}