// Bi.n.d.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.BindingEx.WindowsPhone
{
    using System.Collections.Generic;

#if WINDOWS_PHONE || WINDOWS_WPF
    using System.Windows;
#endif

#if NETFX_CORE
    using Windows.UI.Xaml;
#endif

    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;

    // ReSharper disable InconsistentNaming
    public static class Bi
    // ReSharper restore InconsistentNaming
    {
        static Bi()
        {
            MvxDesignTimeChecker.Check();
        }

        // ReSharper disable InconsistentNaming
        public static readonly DependencyProperty ndProperty =
            // ReSharper restore InconsistentNaming
            DependencyProperty.RegisterAttached("nd",
                                                typeof(string),
                                                typeof(Bi),
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
                _bindingCreator = _bindingCreator ?? ResolveBindingCreator();
                return _bindingCreator;
            }
        }

        private static IMvxBindingCreator ResolveBindingCreator()
        {
            IMvxBindingCreator toReturn;
            if (!Mvx.TryResolve<IMvxBindingCreator>(out toReturn))
            {
                throw new MvxException("Unable to resolve the binding creator - have you initialized Windows Binding");
            }

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