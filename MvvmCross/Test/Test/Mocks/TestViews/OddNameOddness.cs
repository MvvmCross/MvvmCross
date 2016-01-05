namespace MvvmCross.Test.Mocks.TestViews
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public class OddNameOddness : IMvxView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}