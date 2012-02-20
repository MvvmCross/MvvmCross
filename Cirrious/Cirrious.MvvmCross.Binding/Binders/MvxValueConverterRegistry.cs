using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Interfaces.Converters;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxValueConverterRegistry
        : IMvxValueConverterProvider
        , IMvxValueConverterRegistry
    {
        private readonly Dictionary<string, IMvxValueConverter> _converters = new Dictionary<string, IMvxValueConverter>();

        public IMvxValueConverter Find(string converterName)
        {
            IMvxValueConverter toReturn;
            if (!_converters.TryGetValue(converterName, out toReturn))
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Warning,"Could not find named converter " + converterName);
            }
            return toReturn;
        }

        public void AddOrOverwrite(string name, IMvxValueConverter converter)
        {
            _converters[name] = converter;
        }
    }
}