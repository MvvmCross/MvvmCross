// IMvxBindingCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#if WINDOWS_PHONE || WINDOWS_WPF
#endif

#if NETFX_CORE

using Windows.UI.Xaml;

#endif


namespace MvvmCross.BindingEx.WindowsPhone

{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using global::MvvmCross.Binding.Bindings;

    public interface IMvxBindingCreator
    {
        void CreateBindings(
            object sender,
            DependencyPropertyChangedEventArgs args,
            Func<string, IEnumerable<MvxBindingDescription>> parseBindingDescriptions);
    }
}