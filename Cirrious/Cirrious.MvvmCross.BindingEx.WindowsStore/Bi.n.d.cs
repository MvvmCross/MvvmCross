﻿// Bi.n.d.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
#if WINDOWS_PHONE
using System.Windows;
using Cirrious.MvvmCross.BindingEx.WindowsPhone;
#endif
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;
#if NETFX_CORE
using Windows.UI.Xaml;
using Cirrious.MvvmCross.BindingEx.WindowsStore;
#endif

// ReSharper disable CheckNamespace
namespace mvx
// ReSharper restore CheckNamespace
{
// ReSharper disable InconsistentNaming
    public static class Bi
// ReSharper restore InconsistentNaming
    {
// ReSharper disable InconsistentNaming
        public static readonly DependencyProperty ndProperty =
// ReSharper restore InconsistentNaming
            DependencyProperty.RegisterAttached("nd",
                                                typeof (string),
                                                typeof (Bi),
                                                new PropertyMetadata(null, CallBackWhenndIsChanged));

        public static string Getnd(DependencyObject obj)
        {
            return obj.GetValue(ndProperty) as string;
        }

        public static void Setnd(
            DependencyObject obj,
            string value)
        {
            obj.SetValue(ndProperty, value);
        }

        private static IMvxBindingCreator _bindingCreator;
        private static IMvxBindingCreator BindingCreator
        {
            get
            {
                _bindingCreator = _bindingCreator ?? Mvx.Resolve<IMvxBindingCreator>();
                return _bindingCreator;
            }
        }

        private static void CallBackWhenndIsChanged(
            object sender,
            DependencyPropertyChangedEventArgs args)
        {
            // bindingCreator may be null in the designer currently 
            var bindingCreator = BindingCreator;
            if (bindingCreator == null)
                return;

            bindingCreator.CreateBindings(sender, args, ParseBindingDescriptions);
        }

        private static IEnumerable<MvxBindingDescription> ParseBindingDescriptions(string bindingText)
        {
            if (MvxSingleton<IMvxBindingSingletonCache>.Instance == null)
                return null;

            return MvxSingleton<IMvxBindingSingletonCache>.Instance.BindingDescriptionParser.Parse(bindingText);
        }
    }
}