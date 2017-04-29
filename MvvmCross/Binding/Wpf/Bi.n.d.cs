﻿// Bi.n.d.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
#if WINDOWS_COMMON
using Windows.UI.Xaml;

namespace MvvmCross.BindingEx.WindowsCommon
#endif
#if WINDOWS_WPF
using System.Windows;

namespace MvvmCross.BindingEx.Wpf
#endif
{
    // ReSharper disable InconsistentNaming
    public static class Bi
        // ReSharper restore InconsistentNaming
    {
// ReSharper disable InconsistentNaming
        public static readonly DependencyProperty ndProperty =
            // ReSharper restore InconsistentNaming
            DependencyProperty.RegisterAttached("nd",
                typeof(string),
                typeof(Bi),
                new PropertyMetadata(null, CallBackWhenndIsChanged));

        private static IMvxBindingCreator _bindingCreator;

        static Bi()
        {
            MvxDesignTimeChecker.Check();
        }

        private static IMvxBindingCreator BindingCreator
        {
            get
            {
                _bindingCreator = _bindingCreator ?? ResolveBindingCreator();
                return _bindingCreator;
            }
        }

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

        private static IMvxBindingCreator ResolveBindingCreator()
        {
            IMvxBindingCreator toReturn;
            if (!Mvx.TryResolve(out toReturn))
                throw new MvxException("Unable to resolve the binding creator - have you initialized Windows Binding");

            return toReturn;
        }

        private static void CallBackWhenndIsChanged(
            object sender,
            DependencyPropertyChangedEventArgs args)
        {
            // bindingCreator may be null in the designer currently
            var bindingCreator = BindingCreator;

            bindingCreator?.CreateBindings(sender, args, ParseBindingDescriptions);
        }

        private static IEnumerable<MvxBindingDescription> ParseBindingDescriptions(string bindingText)
        {
            if (MvxSingleton<IMvxBindingSingletonCache>.Instance == null)
                return null;

            return MvxSingleton<IMvxBindingSingletonCache>.Instance.BindingDescriptionParser.Parse(bindingText);
        }
    }
}