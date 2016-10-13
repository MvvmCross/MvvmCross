namespace MvvmCross.Binding.iOS.ViewModels
{
    using Core.ViewModels;

    public interface IMvxPageViewModel : IMvxViewModel
    {
        int PageIndex { get; }
    }
}
