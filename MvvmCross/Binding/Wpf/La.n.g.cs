﻿// La.n.g.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#if WINDOWS_COMMON
using Windows.UI.Xaml;

namespace MvvmCross.BindingEx.WindowsCommon
#endif

#if WINDOWS_WPF
using System;
using System.Windows;
using System.Collections.Generic;

using MvvmCross.Binding.Bindings;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;

namespace MvvmCross.Binding.Wpf
#endif
{
    // ReSharper disable InconsistentNaming
    public static class La
    // ReSharper restore InconsistentNaming
    {
        static La()
        {
            MvxDesignTimeChecker.Check();
        }

        // ReSharper disable InconsistentNaming
        public static readonly DependencyProperty ngProperty =
            // ReSharper restore InconsistentNaming
            DependencyProperty.RegisterAttached("ng",
                                                typeof(string),
                                                typeof(La),
                                                new PropertyMetadata(null, CallBackWhenngIsChanged));

        public static string Getng(DependencyObject obj)
        {
            return obj.GetValue(ngProperty) as string;
        }

        public static void Setng(
            DependencyObject obj,
            string value)
        {
            obj.SetValue(ngProperty, value);
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

        private static void CallBackWhenngIsChanged(
            object sender,
            DependencyPropertyChangedEventArgs args)
        {
            // bindingCreator may be null in the designer currently
            var bindingCreator = BindingCreator;
            if (bindingCreator == null)
                return;

            bindingCreator.CreateBindings(sender, args, ParseBindingDescriptions);
        }

        private static IEnumerable<MvxBindingDescription> ParseBindingDescriptions(string languageText)
        {
            if (MvxSingleton<IMvxBindingSingletonCache>.Instance == null)
                return null;

            return MvxSingleton<IMvxBindingSingletonCache>.Instance.BindingDescriptionParser.LanguageParse(languageText);
        }
    }
}