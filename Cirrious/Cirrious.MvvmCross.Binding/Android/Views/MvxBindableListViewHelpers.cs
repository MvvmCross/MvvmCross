using Android.Content;
using Android.Util;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public static class MvxBindableListViewHelpers
    {
        public static int ReadTemplatePath(Context context, IAttributeSet attrs)
        {
            var typedArray = context.ObtainStyledAttributes(attrs, MvxAndroidBindingResource.Instance.BindableListViewStylableGroupId);

            try
            {
                var numStyles = typedArray.IndexCount;
                for (var i = 0; i < numStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);
                    if (attributeId == MvxAndroidBindingResource.Instance.BindableListItemTemplateId)
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