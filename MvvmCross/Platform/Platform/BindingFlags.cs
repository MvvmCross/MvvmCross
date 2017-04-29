using System;

namespace MvvmCross.Platform
{
    [Flags]
    public enum BindingFlags
    {
        None = 0,
        Instance = 1,
        Public = 2,
        Static = 4,
        FlattenHierarchy = 8,
        SetProperty = 8192
    }
}