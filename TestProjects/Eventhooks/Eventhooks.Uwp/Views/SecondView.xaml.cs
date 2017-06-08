using Eventhooks.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventhooks.Uwp.Views
{
    [MvxViewFor(typeof(SecondViewModel))]
    public sealed partial class SecondView : MvxWindowsPage
    {
        public SecondView()
        {
            InitializeComponent();
        }

        public SecondViewModel SecondViewModel
        {
            get => (SecondViewModel)ViewModel;
        }
    }
}
