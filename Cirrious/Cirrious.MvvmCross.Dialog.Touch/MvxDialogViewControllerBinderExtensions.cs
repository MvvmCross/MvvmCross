// MvxDialogViewControllerBinderExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;

namespace Cirrious.MvvmCross.Dialog.Touch
{
    public static class MvxDialogViewControllerBinderExtensions
    {
        public static T Bind<T>(this T element, IMvxTouchView touchView, string descriptionText)
        {
            touchView.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxTouchView touchView,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            touchView.AddBindings(element, descriptions);
            return element;
        }

        /*
        public static T Bind<T>(this T element, IMvxTouchView touchView, object source, string descriptionText)
        {
            touchView.AddBindings(source, element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxTouchView touchView, object source,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            touchView.AddBindings(source, element, descriptions);
            return element;
        }
		*/
    }
}