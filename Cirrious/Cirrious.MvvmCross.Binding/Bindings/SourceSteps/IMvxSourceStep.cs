// IMvxSourceStep.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Bindings.Source;

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public interface IMvxSourceStep : IMvxBinding
    {
        Type TargetType { get; set; }
        Type SourceType { get; }
        void SetValue(object value);
        event EventHandler<MvxSourcePropertyBindingEventArgs> Changed;
        bool TryGetValue(out object value);
        object DataContext { get; set; }
    }
}