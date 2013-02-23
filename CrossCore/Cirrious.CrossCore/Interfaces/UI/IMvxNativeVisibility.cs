// IMvxNativeVisibility.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;

namespace Cirrious.CrossCore.Interfaces.UI
{
    public interface IMvxNativeVisibility
    {
        object ToNative(MvxVisibility visibility);
    }
}