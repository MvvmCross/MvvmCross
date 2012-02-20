namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction
{
    public interface IMvxTargetBindingFactory
    {
        IMvxTargetBinding CreateBinding(object target, MvxBindingDescription description);
    }
}