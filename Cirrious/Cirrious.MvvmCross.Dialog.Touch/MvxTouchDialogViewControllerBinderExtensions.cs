// MvxTouchDialogViewControllerBinderExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;

namespace Cirrious.MvvmCross.Dialog.Touch
{
    public static class MvxTouchDialogViewControllerBinderExtensions
    {
        public static T Bind<T>(this T element, IMvxBindingTouchView touchView, string descriptionText)
        {
            touchView.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingTouchView touchView,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            touchView.AddBindings(element, descriptions);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingTouchView touchView, object source, string descriptionText)
        {
            touchView.AddBindings(source, element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingTouchView touchView, object source,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            touchView.AddBindings(source, element, descriptions);
            return element;
        }
    }
}