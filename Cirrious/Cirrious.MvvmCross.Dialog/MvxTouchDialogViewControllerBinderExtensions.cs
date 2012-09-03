#region Copyright
// <copyright file="MvxTouchDialogViewControllerBinderExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Dialog.Touch
{
    public static class MvxTouchDialogViewControllerBinderExtensions
    {
        public static T Bind<T>(this T element, IMvxBindingTouchView touchView, string descriptionText)
        {
            touchView.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingTouchView touchView, IEnumerable<MvxBindingDescription> descriptions)
        {
            touchView.AddBindings(element, descriptions);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingTouchView touchView, object source, string descriptionText)
        {
            touchView.AddBindings(source, element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingTouchView touchView, object source, IEnumerable<MvxBindingDescription> descriptions)
        {
            touchView.AddBindings(source, element, descriptions);
            return element;
        }
    }
}