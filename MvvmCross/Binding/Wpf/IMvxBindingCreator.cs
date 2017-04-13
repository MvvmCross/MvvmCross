// IMvxBindingCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

using MvvmCross.Binding.Bindings;

#if WINDOWS_COMMON
using Windows.UI.Xaml;

namespace MvvmCross.BindingEx.WindowsCommon
#endif

#if WINDOWS_WPF
using System.Windows;

namespace MvvmCross.BindingEx.Wpf
#endif
{
    public interface IMvxBindingCreator
    {
        void CreateBindings(
            object sender,
            DependencyPropertyChangedEventArgs args,
            Func<string, IEnumerable<MvxBindingDescription>> parseBindingDescriptions);
    }
}