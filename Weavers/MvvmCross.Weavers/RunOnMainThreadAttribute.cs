using System;

namespace MvxMainThread
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RunOnMainThreadAttribute : Attribute { }
}
