// IMvxBindingOwner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Touch.Interfaces
{
    public interface IMvxBindingOwner
        : IMvxDataConsumer
    {
#warning Unify this with the Android project - shouldbe easy? Well, should be possible...
        List<IMvxUpdateableBinding> Bindings { get; }
    }
}