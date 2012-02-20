using Cirrious.MvvmCross.Interfaces.Converters;

namespace Cirrious.MvvmCross.Binding.Interfaces.Binders
{
    public interface IMvxValueConverterProvider
    {
        IMvxValueConverter Find(string converterName);
    }
}