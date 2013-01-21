using System;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Interfaces.Converters;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxInstanceBasedValueConverterRegistryFiller
    {
        private readonly IMvxValueConverterRegistry _registry;

        public MvxInstanceBasedValueConverterRegistryFiller(IMvxValueConverterRegistry registry)
        {
            _registry = registry;
        }

        public void AddFieldConverters(Type type)
        {
            var instance = Activator.CreateInstance(type);

            var pairs = from field in type.GetFields()
                        where !field.IsStatic
                        where field.IsPublic
                        where typeof (IMvxValueConverter).IsAssignableFrom(field.FieldType)
                        let converter = field.GetValue(instance) as IMvxValueConverter
                        where converter != null
                        select new
                            {
                                field.Name,
                                Converter = converter
                            };

            foreach (var pair in pairs)
            {
                _registry.AddOrOverwrite(pair.Name, pair.Converter);
            }
        }
    }
}