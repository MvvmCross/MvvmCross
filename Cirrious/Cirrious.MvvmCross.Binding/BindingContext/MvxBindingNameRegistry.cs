using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxBindingNameRegistry
        : IMvxBindingNameLookup
          , IMvxBindingNameRegistry
    {
        private readonly Dictionary<Type, string> _lookup = new Dictionary<Type,string>();

        public string DefaultFor (Type type)
        {
            string toReturn;
            TryDefaultFor(type, out toReturn, true);
            return toReturn;
        }

        private bool TryDefaultFor(Type type, out string toReturn, bool includeInterfaces = true)
        {
            if (type == typeof(Object))
            {
                toReturn = null;
                return false;
            }

            if (_lookup.TryGetValue (type, out toReturn))
                return true;

            if (type.IsInterface)
                return false;

            if (includeInterfaces)
            {
                var interfaces = type.GetInterfaces();
                foreach (var iface in interfaces)
                {
                    if (TryDefaultFor(iface, out toReturn, false))
                        return true;
                }
            }

            return TryDefaultFor(type.BaseType, out toReturn, false);
        }

        public void AddOrOverwrite (Type type, string name)
        {
            _lookup [type] = name;
        }
    }
}