// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Util;
using MvvmCross.Platforms.Android.Binding.ResourceHelpers;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    public static class MvxAttributeHelpers
    {
        private static readonly Lazy<IMvxAndroidBindingResource> mvxAndroidBindingResource = new Lazy<IMvxAndroidBindingResource>(() => Mvx.IoCProvider.GetSingleton<IMvxAndroidBindingResource>());

        public static int ReadDropDownListItemTemplateId(Context context, IAttributeSet attrs)
        {
            return ReadAttributeValue(context, attrs,
                                      mvxAndroidBindingResource.Value.ListViewStylableGroupId,
                                      mvxAndroidBindingResource.Value.DropDownListItemTemplateId);
        }

        public static int ReadListItemTemplateId(Context context, IAttributeSet attrs)
        {
            return ReadAttributeValue(context, attrs,
                                      mvxAndroidBindingResource.Value.ListViewStylableGroupId,
                                      mvxAndroidBindingResource.Value.ListItemTemplateId);
        }

        public static int ReadTemplateId(Context context, IAttributeSet attrs)
        {
            return ReadAttributeValue(context, attrs,
                                      mvxAndroidBindingResource.Value.ControlStylableGroupId,
                                      mvxAndroidBindingResource.Value.TemplateId);
        }

        public static int ReadGroupItemTemplateId(Context context, IAttributeSet attrs)
        {
            return ReadAttributeValue(context, attrs,
                                      mvxAndroidBindingResource.Value.ExpandableListViewStylableGroupId,
                                      mvxAndroidBindingResource.Value.GroupItemTemplateId);
        }

        public static int ReadAttributeValue(Context context, IAttributeSet attrs, int[] groupId,
                                             int requiredAttributeId)
        {
            var typedArray = context.ObtainStyledAttributes(attrs, groupId);

            try
            {
                var numStyles = typedArray.IndexCount;
                for (var i = 0; i < numStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);
                    if (attributeId == requiredAttributeId)
                    {
                        return typedArray.GetResourceId(attributeId, 0);
                    }
                }
                return 0;
            }
            finally
            {
                typedArray.Recycle();
            }
        }
    }
}
