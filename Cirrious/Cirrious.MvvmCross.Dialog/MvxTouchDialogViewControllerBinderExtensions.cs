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
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Dialog.Touch.Dialog;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Touch.Dialog
{
    public static class MvxTouchDialogViewControllerBinderExtensions
    {
        public static Element Bind<TViewModel>(this Element element, MvxTouchDialogViewController<TViewModel> controller, string descriptionText)
            where TViewModel : class, IMvxViewModel
        {
            controller.AddBindings(element, descriptionText);
            return element;
        }

        public static Element Bind<TViewModel>(this Element element, MvxTouchDialogViewController<TViewModel> controller, IEnumerable<MvxBindingDescription> descriptions)
            where TViewModel : class, IMvxViewModel
        {
            controller.AddBindings(element, descriptions);
            return element;
        }        
    }
}