using Cirrious.MvvmCross.Interfaces.Converters;

namespace Cirrious.MvvmCross.Binding.Interfaces.Binders
{
    public interface IMvxValueConverterRegistry
    {
        void AddOrOverwrite(string name, IMvxValueConverter converter);
    }
}