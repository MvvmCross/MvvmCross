using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Test.TestViewModels;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Test.TestViews
{
    public class Test1View : IMvxView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
    public class NotTest2View : IMvxView
    {
        public object DataContext { get; set; }
        IMvxViewModel IMvxView.ViewModel { get; set; }
        public Test2ViewModel ViewModel { get; set; }
    }
    [MvxViewFor(typeof(Test3ViewModel))]
    public class NotTest3View : IMvxView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }

    public abstract class AbstractTest1View : IMvxView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
    public class NotReallyAView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}
