using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxNamedInstanceRegistry<T>
        : IMvxNamedInstanceLookup<T>
          , IMvxNamedInstanceRegistry<T>
        where T : class
    {
        private readonly Dictionary<string, T> _converters =
            new Dictionary<string, T>();

        public T Find(string converterName)
        {
            if (string.IsNullOrEmpty(converterName))
                return null;

            T toReturn;
            if (!_converters.TryGetValue(converterName, out toReturn))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Could not find named {0} for {1}", converterName, typeof(T).Name);
            }
            return toReturn;
        }

        public void AddOrOverwrite(string name, T converter)
        {
            _converters[name] = converter;
        }

        public void AddOrOverwriteFrom(Assembly assembly)
        {
            this.Fill(assembly);
        }
    }
}