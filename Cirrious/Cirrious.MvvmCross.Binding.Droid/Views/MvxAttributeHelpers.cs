// MvxAttributeHelpers.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Util;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public static class MvxAttributeHelpers
    {
        public static int ReadDropDownListItemTemplateId(Context context, IAttributeSet attrs)
        {
            return ReadAttributeValue(context, attrs,
                                                                   MvxAndroidBindingResource
                                                                       .Instance
                                                                       .ListViewStylableGroupId,
                                                                   MvxAndroidBindingResource
                                                                       .Instance
                                                                       .DropDownListItemTemplateId);
        }

        public static int ReadListItemTemplateId(Context context, IAttributeSet attrs)
        {
            return ReadAttributeValue(context, attrs,
                                                   MvxAndroidBindingResource.Instance
                                                                            .ListViewStylableGroupId,
                                                   MvxAndroidBindingResource.Instance
                                                                            .ListItemTemplateId);
        }

        public static int ReadTemplateId(Context context, IAttributeSet attrs)
        {
            return ReadAttributeValue(context, attrs,
                                                         MvxAndroidBindingResource.Instance
                                                                                  .ControlStylableGroupId,
                                                         MvxAndroidBindingResource.Instance
                                                                                  .TemplateId);
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