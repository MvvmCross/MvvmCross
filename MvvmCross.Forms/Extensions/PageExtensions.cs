using System;
using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using XFPage = Xamarin.Forms.Page;

namespace MvvmCross.Forms.Presenters
{
    public static class PageExtensions
    {
        public static void SetModalPagePresentationStyle(this XFPage page, MvxFormsModalPresentationStyle presentationStyle)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            page.On<iOS>().SetModalPresentationStyle((UIModalPresentationStyle)presentationStyle);
        }
    }
}
