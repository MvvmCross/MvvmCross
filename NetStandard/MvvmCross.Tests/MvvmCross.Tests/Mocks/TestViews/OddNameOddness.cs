using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Test.Mocks.TestViews
{
    public class OddNameOddness : IMvxView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}