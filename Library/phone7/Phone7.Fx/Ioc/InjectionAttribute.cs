using System;

namespace Phone7.Fx.Ioc
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class InjectionAttribute : Attribute
    {
        public InjectionAttribute()
        {
        }

    }
}