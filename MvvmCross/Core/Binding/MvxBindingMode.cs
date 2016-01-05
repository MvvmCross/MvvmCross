// MvxBindingMode.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding
{
    public enum MvxBindingMode
    {
        Default = 0,
        TwoWay,
        OneWay,
        OneTime,
        OneWayToSource
    }
}