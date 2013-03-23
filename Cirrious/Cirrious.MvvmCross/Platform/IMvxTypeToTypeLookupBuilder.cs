using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.MvvmCross.Platform
{
    public interface IMvxTypeToTypeLookupBuilder
    {
        IDictionary<Type, Type> Build(Assembly[] sourceAssemblies);
    }
}