using System;

namespace Cirrious.MvvmCross.IoC
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class MvxOpenNetCfInjectionAttribute : Attribute
    {
        public MvxOpenNetCfInjectionAttribute()
        {
        }

    }
}