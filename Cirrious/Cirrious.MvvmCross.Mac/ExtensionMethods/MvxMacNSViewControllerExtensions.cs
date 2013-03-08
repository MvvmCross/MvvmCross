#region Copyright
// <copyright file="MvxTouchUIViewControllerExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Mac.ExtensionMethods
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