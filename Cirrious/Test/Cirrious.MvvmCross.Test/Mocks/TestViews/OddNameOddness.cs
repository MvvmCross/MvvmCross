using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Test.Mocks.TestViews
{
    public class OddNameOddness : IMvxView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}