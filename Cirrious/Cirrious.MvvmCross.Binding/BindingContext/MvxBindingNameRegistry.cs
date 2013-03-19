using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;

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
            _lookup.TryGetValue (type, out toReturn);
            return toReturn;
        }

        public void AddOrOverwrite (Type type, string name)
        {
            _lookup [type] = name;
        }
    }
}