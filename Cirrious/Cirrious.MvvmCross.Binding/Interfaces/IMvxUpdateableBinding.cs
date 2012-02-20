namespace Cirrious.MvvmCross.Binding.Interfaces
{
    public interface IMvxUpdateableBinding : IMvxBinding
    {
        object DataContext { get; set; }
    }
}