// IMvxBindingCreator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Windows;
using Cirrious.MvvmCross.Binding.Binders;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone
{
    public interface IMvxBindingCreator
    {
        void CreateBindings(
            object sender, 
            DependencyPropertyChangedEventArgs args,
            Func<string, IEnumerable<MvxBindingDescription>> parseBindingDescriptions);
    }
}