#region Copyright
// <copyright file="MvxBindableListViewHelpers.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Android.Content;
using Android.Util;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public static class MvxBindableListViewHelpers
    {
        public static int ReadAttributeValue(Context context, IAttributeSet attrs, int[] groupId, int requiredAttributeId)
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