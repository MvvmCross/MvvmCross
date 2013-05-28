using System;
using Cirrious.CrossCore.Converters;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxAutoValueConverters
    {
        IMvxValueConverter Find(Type viewModelType, Type viewType);
        void Register(Type viewModelType, Type viewType, IMvxValueConverter converter);
    }
}