using System;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxSourceStepFactoryRegistry : IMvxSourceStepFactory
    {
        void AddOrOverwrite(Type type, IMvxSourceStepFactory factory);
    }
}