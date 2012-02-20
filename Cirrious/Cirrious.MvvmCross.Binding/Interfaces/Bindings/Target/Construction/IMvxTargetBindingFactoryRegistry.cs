namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction
{
    public interface IMvxTargetBindingFactoryRegistry : IMvxTargetBindingFactory
    {
        void RegisterFactory(IMvxPluginTargetBindingFactory factory);
    }
}