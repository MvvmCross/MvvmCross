// IMvxBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Binding.Touch.Interfaces.BindingContext
{
    public interface IMvxBindingContext
        : IMvxBaseBindingContext<NSView>
    {
    }
}