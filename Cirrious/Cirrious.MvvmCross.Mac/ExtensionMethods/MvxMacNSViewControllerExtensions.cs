// MvxMacNSViewControllerExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Touch.ExtensionMethods
{
    public static class MvxMacViewControllerExtensions
    {
        public static bool IsVisible(this NSWindowController controller)
        {
#warning Need to work out how to implement this properly - or need to remove the need for it from the framework
            return true;
        }
    }
}